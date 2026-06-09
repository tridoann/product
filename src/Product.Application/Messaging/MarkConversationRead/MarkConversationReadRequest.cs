using MediatR;

namespace Product.Application.Messaging.MarkConversationRead;

public class MarkConversationReadRequest : IRequest<MarkConversationReadResponse>
{
    public int ConversationId { get; set; }
    public int UserId { get; set; }
}
