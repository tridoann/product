using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Messaging.MarkConversationRead;

public class MarkConversationReadHandler(IConversationRepository conversationRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<MarkConversationReadRequest, MarkConversationReadResponse>
{
    public async Task<MarkConversationReadResponse> Handle(MarkConversationReadRequest request, CancellationToken cancellationToken)
    {
        var conv = await conversationRepository.GetWithParticipantsAsync(request.ConversationId, cancellationToken)
            ?? throw new KeyNotFoundException("Conversation not found.");

        var participant = conv.Participants.FirstOrDefault(p => p.UserId == request.UserId)
            ?? throw new UnauthorizedAccessException("Not a participant.");

        participant.LastReadAt = DateTime.UtcNow;
        await unitOfWork.CommitAsync(cancellationToken);

        return new MarkConversationReadResponse();
    }
}
