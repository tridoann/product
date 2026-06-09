using MediatR;

namespace Product.Application.Commerce.Orders.PlaceOrder;

public class PlaceOrderRequest : IRequest<PlaceOrderResponse>
{
    public int BuyerId { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
}
