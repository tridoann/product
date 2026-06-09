using MediatR;
using Product.Domain.Enums;

namespace Product.Application.Commerce.Orders.UpdateOrderStatus;

public class UpdateOrderStatusRequest : IRequest<UpdateOrderStatusResponse>
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}
