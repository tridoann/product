using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Commerce.Cart.RemoveFromCart;

public class RemoveFromCartHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveFromCartRequest, RemoveFromCartResponse>
{
    public async Task<RemoveFromCartResponse> Handle(RemoveFromCartRequest request, CancellationToken cancellationToken)
    {
        var item = await cartItemRepository.GetAsync(request.CartItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Cart item not found.");

        var cart = await cartRepository.GetAsync(item.CartId, cancellationToken);
        if (cart?.UserId != request.UserId)
            throw new UnauthorizedAccessException("Item does not belong to your cart.");

        cartItemRepository.Remove(item);
        await unitOfWork.CommitAsync(cancellationToken);
        return new RemoveFromCartResponse();
    }
}
