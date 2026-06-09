using Product.Domain.Enums;

namespace Product.Application.Support.GetTicket;

public class GetTicketResponse
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public int SubmittedById { get; set; }
    public string SubmittedByUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public List<TicketReplyDto> Replies { get; set; } = [];
}

public class TicketReplyDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public string AuthorDisplayName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsAdminReply { get; set; }
    public DateTime CreatedAt { get; set; }
}
