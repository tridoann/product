using MediatR;
using Product.Application.Exceptions;
using Product.Domain.Repositories;

namespace Product.Application.Support.GetTicket;

public class GetTicketHandler(ISupportTicketRepository ticketRepository)
    : IRequestHandler<GetTicketRequest, GetTicketResponse>
{
    public async Task<GetTicketResponse> Handle(GetTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetWithRepliesAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException($"Ticket {request.TicketId} not found.");

        if (!request.IsAdmin && ticket.SubmittedById != request.RequesterId)
            throw new UnauthorizedAccessException("Access denied.");

        return new GetTicketResponse
        {
            Id = ticket.Id,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            SubmittedById = ticket.SubmittedById,
            SubmittedByUsername = ticket.SubmittedBy.Username,
            CreatedAt = ticket.CreatedAt,
            ResolvedAt = ticket.ResolvedAt,
            Replies = ticket.Replies.OrderBy(r => r.CreatedAt).Select(r => new TicketReplyDto
            {
                Id = r.Id,
                AuthorId = r.AuthorId,
                AuthorUsername = r.Author.Username,
                AuthorDisplayName = r.Author.DisplayName,
                Content = r.Content,
                IsAdminReply = r.IsAdminReply,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }
}
