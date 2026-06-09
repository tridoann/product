using MediatR;
using Product.Domain.Enums;

namespace Product.Application.Support.CreateTicket;

public class CreateTicketRequest : IRequest<CreateTicketResponse>
{
    public int SubmittedById { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
}
