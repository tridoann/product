using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Messaging.GetMessages;

public class GetMessagesHandler(IMessageRepository messageRepository, IConversationRepository conversationRepository)
    : IRequestHandler<GetMessagesRequest, GetMessagesResponse>
{
    public async Task<GetMessagesResponse> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
    {
        var conv = await conversationRepository.GetWithParticipantsAsync(request.ConversationId, cancellationToken)
            ?? throw new KeyNotFoundException("Conversation not found.");

        if (!conv.Participants.Any(p => p.UserId == request.UserId))
            throw new UnauthorizedAccessException("Not a participant of this conversation.");

        var paged = await messageRepository.GetConversationMessagesAsync(request.ConversationId, request.PageIndex, request.PageSize, cancellationToken);

        return new GetMessagesResponse
        {
            TotalCount = paged.TotalCount,
            PageIndex = paged.PageIndex,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderDisplayName = m.Sender.DisplayName,
                SenderAvatarUrl = m.Sender.AvatarUrl,
                Content = m.Content,
                MediaUrl = m.MediaUrl,
                CreatedAt = m.CreatedAt
            }).ToList()
        };
    }
}
