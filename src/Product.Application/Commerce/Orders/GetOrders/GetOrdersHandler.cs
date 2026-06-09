using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Commerce.Orders.GetOrders;

public class GetOrdersHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrdersRequest, GetOrdersResponse>
{
    public async Task<GetOrdersResponse> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var paged = request.IsAdmin
            ? await orderRepository.GetAllAsync(request.PageIndex, request.PageSize, cancellationToken)
            : await orderRepository.GetByUserAsync(request.UserId, request.PageIndex, request.PageSize, cancellationToken);

        return new GetOrdersResponse
        {
            TotalCount = paged.TotalCount,
            PageIndex = paged.PageIndex,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(o => new OrderDto
            {
                Id = o.Id,
                BuyerId = o.BuyerId,
                BuyerUsername = o.Buyer?.Username,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                ShippingAddress = o.ShippingAddress,
                ItemCount = o.Items.Count,
                CreatedAt = o.CreatedAt
            }).ToList()
        };
    }
}
