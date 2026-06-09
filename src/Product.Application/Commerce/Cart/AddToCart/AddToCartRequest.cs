using MediatR;

namespace Product.Application.Commerce.Cart.AddToCart;

public class AddToCartRequest : IRequest<AddToCartResponse>
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}
