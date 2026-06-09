using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Commerce.Orders.PlaceOrder;

public class PlaceOrderHandler(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<PlaceOrderRequest, PlaceOrderResponse>
{
    public async Task<PlaceOrderResponse> Handle(PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdAsync(request.BuyerId, cancellationToken);
        if (cart is null || !cart.Items.Any())
            throw new InvalidOperationException("Cart is empty.");

        // Validate stock
        foreach (var item in cart.Items)
        {
            var product = await productRepository.GetAsync(item.ProductId, cancellationToken)
                ?? throw new KeyNotFoundException($"Product {item.ProductId} not found.");
            if (!product.IsActive || product.StockQuantity < item.Quantity)
                throw new InvalidOperationException($"Insufficient stock for {product.Name}.");
        }

        // Build order
        var orderItems = cart.Items.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.Product.Price
        }).ToList();

        var order = new Order
        {
            BuyerId = request.BuyerId,
            ShippingAddress = request.ShippingAddress,
            TotalAmount = orderItems.Sum(i => i.UnitPrice * i.Quantity),
            Items = orderItems
        };

        await orderRepository.CreateAsync(order, cancellationToken);

        // Decrement stock and clear cart
        foreach (var item in cart.Items)
        {
            var product = await productRepository.GetAsync(item.ProductId, cancellationToken)!;
            product!.StockQuantity -= item.Quantity;
            cartItemRepository.Remove(item);
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return new PlaceOrderResponse
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };
    }
}
