using MediatR;

namespace Product.Application.Messaging.GetMessages;

public class GetMessagesRequest : IRequest<GetMessagesResponse>
{
    public int ConversationId { get; set; }
    public int UserId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
