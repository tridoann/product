using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Support.ReplyToTicket;

public class ReplyToTicketHandler(ISupportTicketRepository ticketRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<ReplyToTicketRequest, ReplyToTicketResponse>
{
    public async Task<ReplyToTicketResponse> Handle(ReplyToTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(request.TicketId, cancellationToken)
            ?? throw new KeyNotFoundException("Ticket not found.");

        var reply = new TicketReply
        {
            TicketId = ticket.Id,
            AuthorId = request.AuthorId,
            Content = request.Content,
            IsAdminReply = request.IsAdminReply
        };

        ticket.Replies.Add(reply);
        await unitOfWork.CommitAsync(cancellationToken);

        return new ReplyToTicketResponse { Id = reply.Id, Content = reply.Content, IsAdminReply = reply.IsAdminReply, CreatedAt = reply.CreatedAt };
    }
}
