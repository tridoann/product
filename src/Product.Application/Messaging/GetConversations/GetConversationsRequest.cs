using MediatR;

namespace Product.Application.Messaging.GetConversations;

public class GetConversationsRequest : IRequest<GetConversationsResponse>
{
    public int UserId { get; set; }
}
