using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Enums;
using Product.Domain.Repositories;

namespace Product.Application.Support.UpdateTicketStatus;

public class UpdateTicketStatusHandler(ISupportTicketRepository ticketRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTicketStatusRequest, UpdateTicketStatusResponse>
{
    public async Task<UpdateTicketStatusResponse> Handle(UpdateTicketStatusRequest request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(request.TicketId, cancellationToken)
            ?? throw new KeyNotFoundException("Ticket not found.");

        ticket.Status = request.Status;
        if (request.Status == TicketStatus.Resolved)
            ticket.ResolvedAt = DateTime.UtcNow;

        await unitOfWork.CommitAsync(cancellationToken);

        return new UpdateTicketStatusResponse { TicketId = ticket.Id, Status = ticket.Status };
    }
}
