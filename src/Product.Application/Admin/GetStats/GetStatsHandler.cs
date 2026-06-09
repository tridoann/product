using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Admin.GetStats;

public class GetStatsHandler(
    IUserRepository userRepository,
    IOrderRepository orderRepository,
    ISupportTicketRepository ticketRepository,
    IProductRepository productRepository)
    : IRequestHandler<GetStatsRequest, GetStatsResponse>
{
    public async Task<GetStatsResponse> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var totalUsers = await userRepository.CountAsync(cancellationToken);
        var activeUsers = await userRepository.CountActiveAsync(cancellationToken);
        var totalOrders = await orderRepository.CountAsync(cancellationToken);
        var openTickets = await ticketRepository.CountOpenAsync(cancellationToken);
        var totalProducts = await productRepository.CountActiveAsync(cancellationToken);

        return new GetStatsResponse
        {
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            TotalOrders = totalOrders,
            OpenTickets = openTickets,
            TotalProducts = totalProducts,
        };
    }
}
