using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Messaging.GetConversations;

public class GetConversationsHandler(IConversationRepository conversationRepository)
    : IRequestHandler<GetConversationsRequest, GetConversationsResponse>
{
    public async Task<GetConversationsResponse> Handle(GetConversationsRequest request, CancellationToken cancellationToken)
    {
        var conversations = await conversationRepository.GetUserConversationsAsync(request.UserId, cancellationToken);
        var myParticipant = conversations
            .SelectMany(c => c.Participants)
            .Where(p => p.UserId == request.UserId)
            .ToDictionary(p => p.ConversationId, p => p);

        return new GetConversationsResponse
        {
            Items = conversations.Select(c =>
            {
                var lastMsg = c.Messages.MaxBy(m => m.CreatedAt);
                myParticipant.TryGetValue(c.Id, out var me);

                return new ConversationDto
                {
                    Id = c.Id,
                    Type = c.Type,
                    Title = c.Title,
                    Participants = c.Participants.Select(p => new ParticipantDto
                    {
                        UserId = p.UserId,
                        DisplayName = p.User.DisplayName,
                        AvatarUrl = p.User.AvatarUrl,
                        LastReadAt = p.LastReadAt
                    }).ToList(),
                    LastMessageContent = lastMsg?.Content,
                    LastMessageAt = lastMsg?.CreatedAt,
                    UnreadCount = me?.LastReadAt is null
                        ? c.Messages.Count
                        : c.Messages.Count(m => m.CreatedAt > me.LastReadAt)
                };
            }).ToList()
        };
    }
}
