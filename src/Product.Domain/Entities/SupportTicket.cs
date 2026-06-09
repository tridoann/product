using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class SupportTicket : BaseEntity<int>
{
    public int SubmittedById { get; set; }
    public int? AssignedToId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public DateTime? ResolvedAt { get; set; }

    public User SubmittedBy { get; set; } = null!;
    public User? AssignedTo { get; set; }
    public ICollection<TicketReply> Replies { get; set; } = [];
}
