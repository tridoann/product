using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Support.GetTickets;

public class GetTicketsHandler(ISupportTicketRepository ticketRepository)
    : IRequestHandler<GetTicketsRequest, GetTicketsResponse>
{
    public async Task<GetTicketsResponse> Handle(GetTicketsRequest request, CancellationToken cancellationToken)
    {
        var paged = request.IsAdmin
            ? await ticketRepository.GetAllAsync(request.StatusFilter, request.PageIndex, request.PageSize, cancellationToken)
            : await ticketRepository.GetByUserAsync(request.UserId, request.PageIndex, request.PageSize, cancellationToken);

        return new GetTicketsResponse
        {
            TotalCount = paged.TotalCount,
            PageIndex = paged.PageIndex,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(t => new TicketSummaryDto
            {
                Id = t.Id,
                Subject = t.Subject,
                Status = t.Status,
                Priority = t.Priority,
                SubmittedByUsername = t.SubmittedBy?.Username ?? string.Empty,
                CreatedAt = t.CreatedAt
            }).ToList()
        };
    }
}
