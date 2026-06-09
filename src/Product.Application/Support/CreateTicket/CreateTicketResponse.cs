using Product.Domain.Enums;

namespace Product.Application.Support.CreateTicket;

public class CreateTicketResponse
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
