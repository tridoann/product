using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IConversationRepository : IRepository<Conversation, int>
{
    Task<Conversation?> GetDirectConversationAsync(int userId1, int userId2, CancellationToken ct = default);
    Task<List<Conversation>> GetUserConversationsAsync(int userId, CancellationToken ct = default);
    Task<Conversation?> GetWithParticipantsAsync(int id, CancellationToken ct = default);
}
