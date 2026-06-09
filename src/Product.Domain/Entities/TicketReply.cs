namespace Product.Domain.Entities;

public class TicketReply : BaseEntity<int>
{
    public int TicketId { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsAdminReply { get; set; }

    public SupportTicket Ticket { get; set; } = null!;
    public User Author { get; set; } = null!;
}
