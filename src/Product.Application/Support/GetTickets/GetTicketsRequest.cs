using MediatR;
using Product.Domain.Enums;

namespace Product.Application.Support.GetTickets;

public class GetTicketsRequest : IRequest<GetTicketsResponse>
{
    public int UserId { get; set; }
    public bool IsAdmin { get; set; }
    public TicketStatus? StatusFilter { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
