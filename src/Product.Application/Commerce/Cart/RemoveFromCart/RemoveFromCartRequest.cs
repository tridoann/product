using MediatR;

namespace Product.Application.Commerce.Cart.RemoveFromCart;

public class RemoveFromCartRequest : IRequest<RemoveFromCartResponse>
{
    public int CartItemId { get; set; }
    public int UserId { get; set; }
}
