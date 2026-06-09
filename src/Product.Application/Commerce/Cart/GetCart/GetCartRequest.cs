using MediatR;

namespace Product.Application.Commerce.Cart.GetCart;

public class GetCartRequest : IRequest<GetCartResponse>
{
    public int UserId { get; set; }
}
