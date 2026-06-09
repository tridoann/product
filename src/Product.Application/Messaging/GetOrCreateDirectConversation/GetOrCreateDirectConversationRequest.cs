using MediatR;

namespace Product.Application.Messaging.GetOrCreateDirectConversation;

public class GetOrCreateDirectConversationRequest : IRequest<GetOrCreateDirectConversationResponse>
{
    public int UserId { get; set; }
    public int OtherUserId { get; set; }
}
