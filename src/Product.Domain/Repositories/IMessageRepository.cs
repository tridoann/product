using Product.Common.Models;
using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IMessageRepository : IRepository<Message, int>
{
    Task<PagedList<Message>> GetConversationMessagesAsync(int conversationId, int pageIndex, int pageSize, CancellationToken ct = default);
}
