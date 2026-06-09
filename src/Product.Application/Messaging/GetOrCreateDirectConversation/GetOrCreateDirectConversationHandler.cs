using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Enums;
using Product.Domain.Repositories;

namespace Product.Application.Messaging.GetOrCreateDirectConversation;

public class GetOrCreateDirectConversationHandler(IConversationRepository conversationRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<GetOrCreateDirectConversationRequest, GetOrCreateDirectConversationResponse>
{
    public async Task<GetOrCreateDirectConversationResponse> Handle(GetOrCreateDirectConversationRequest request, CancellationToken cancellationToken)
    {
        var existing = await conversationRepository.GetDirectConversationAsync(request.UserId, request.OtherUserId, cancellationToken);
        if (existing is not null)
            return new GetOrCreateDirectConversationResponse { ConversationId = existing.Id, IsNew = false };

        var conversation = new Conversation
        {
            Type = ConversationType.Direct,
            Participants =
            [
                new ConversationParticipant { UserId = request.UserId },
                new ConversationParticipant { UserId = request.OtherUserId }
            ]
        };

        await conversationRepository.CreateAsync(conversation, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new GetOrCreateDirectConversationResponse { ConversationId = conversation.Id, IsNew = true };
    }
}
