using Product.Domain.Enums;

namespace Product.Application.Commerce.Orders.UpdateOrderStatus;

public class UpdateOrderStatusResponse
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}
