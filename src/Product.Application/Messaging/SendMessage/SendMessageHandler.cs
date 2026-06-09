using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Messaging.SendMessage;

public class SendMessageHandler(IMessageRepository messageRepository, IConversationRepository conversationRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<SendMessageRequest, SendMessageResponse>
{
    public async Task<SendMessageResponse> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var conv = await conversationRepository.GetWithParticipantsAsync(request.ConversationId, cancellationToken)
            ?? throw new KeyNotFoundException("Conversation not found.");

        if (!conv.Participants.Any(p => p.UserId == request.SenderId))
            throw new UnauthorizedAccessException("Not a participant of this conversation.");

        var message = new Message
        {
            ConversationId = request.ConversationId,
            SenderId = request.SenderId,
            Content = request.Content,
            MediaUrl = request.MediaUrl
        };

        await messageRepository.CreateAsync(message, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new SendMessageResponse
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };
    }
}
