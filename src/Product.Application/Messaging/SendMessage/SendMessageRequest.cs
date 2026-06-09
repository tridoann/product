using MediatR;

namespace Product.Application.Messaging.SendMessage;

public class SendMessageRequest : IRequest<SendMessageResponse>
{
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
}
