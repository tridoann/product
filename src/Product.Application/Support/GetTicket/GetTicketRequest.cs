using MediatR;

namespace Product.Application.Support.GetTicket;

public class GetTicketRequest : IRequest<GetTicketResponse>
{
    public int TicketId { get; set; }
    public int RequesterId { get; set; }
    public bool IsAdmin { get; set; }
}
