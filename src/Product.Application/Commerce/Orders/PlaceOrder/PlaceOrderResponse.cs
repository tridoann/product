using Product.Domain.Enums;

namespace Product.Application.Commerce.Orders.PlaceOrder;

public class PlaceOrderResponse
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
