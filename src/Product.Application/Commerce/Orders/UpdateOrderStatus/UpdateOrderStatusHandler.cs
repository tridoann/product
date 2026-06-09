using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Commerce.Orders.UpdateOrderStatus;

public class UpdateOrderStatusHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateOrderStatusRequest, UpdateOrderStatusResponse>
{
    public async Task<UpdateOrderStatusResponse> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Order not found.");

        order.Status = request.Status;
        await unitOfWork.CommitAsync(cancellationToken);

        return new UpdateOrderStatusResponse { OrderId = order.Id, Status = order.Status };
    }
}
