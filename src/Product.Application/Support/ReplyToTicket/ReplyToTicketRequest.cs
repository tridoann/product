using MediatR;

namespace Product.Application.Support.ReplyToTicket;

public class ReplyToTicketRequest : IRequest<ReplyToTicketResponse>
{
    public int TicketId { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsAdminReply { get; set; }
}
