namespace Product.Application.Support.ReplyToTicket;

public class ReplyToTicketResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsAdminReply { get; set; }
    public DateTime CreatedAt { get; set; }
}
