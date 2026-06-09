using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Support.CreateTicket;

public class CreateTicketHandler(ISupportTicketRepository ticketRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTicketRequest, CreateTicketResponse>
{
    public async Task<CreateTicketResponse> Handle(CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = new SupportTicket
        {
            SubmittedById = request.SubmittedById,
            Subject = request.Subject,
            Description = request.Description,
            Priority = request.Priority
        };

        await ticketRepository.CreateAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateTicketResponse { Id = ticket.Id, Subject = ticket.Subject, Status = ticket.Status, CreatedAt = ticket.CreatedAt };
    }
}
