using MediatR;

namespace Product.Application.Commerce.Orders.GetOrders;

public class GetOrdersRequest : IRequest<GetOrdersResponse>
{
    public int UserId { get; set; }
    public bool IsAdmin { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
