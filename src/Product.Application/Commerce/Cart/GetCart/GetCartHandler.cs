using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Commerce.Cart.GetCart;

public class GetCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<GetCartRequest, GetCartResponse>
{
    public async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null)
        {
            cart = new Domain.Entities.Cart { UserId = request.UserId };
            await cartRepository.CreateAsync(cart, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return new GetCartResponse
        {
            CartId = cart.Id,
            Items = cart.Items.Select(i => new CartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductImageUrl = i.Product.ImageUrl,
                UnitPrice = i.Product.Price,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
