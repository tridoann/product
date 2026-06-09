using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Commerce.Cart.AddToCart;

public class AddToCartHandler(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<AddToCartRequest, AddToCartResponse>
{
    public async Task<AddToCartResponse> Handle(AddToCartRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsync(request.ProductId, cancellationToken)
            ?? throw new KeyNotFoundException("Product not found.");

        if (!product.IsActive || product.StockQuantity < request.Quantity)
            throw new InvalidOperationException("Insufficient stock.");

        var cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null)
        {
            cart = new Domain.Entities.Cart { UserId = request.UserId };
            await cartRepository.CreateAsync(cart, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }

        var item = await cartItemRepository.GetAsync(cart.Id, request.ProductId, cancellationToken);
        if (item is not null)
        {
            item.Quantity += request.Quantity;
        }
        else
        {
            item = new CartItem { CartId = cart.Id, ProductId = request.ProductId, Quantity = request.Quantity };
            await cartItemRepository.CreateAsync(item, cancellationToken);
        }

        await unitOfWork.CommitAsync(cancellationToken);
        return new AddToCartResponse { CartItemId = item.Id, NewQuantity = item.Quantity };
    }
}
