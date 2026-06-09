using Product.Domain.Enums;

namespace Product.Application.Support.UpdateTicketStatus;

public class UpdateTicketStatusResponse
{
    public int TicketId { get; set; }
    public TicketStatus Status { get; set; }
}
