using Product.Domain.Entities;
using Product.Domain.Enums;

namespace Product.Domain.Repositories;

public interface IFriendRequestRepository : IRepository<FriendRequest, int>
{
    Task<FriendRequest?> GetPendingAsync(int senderId, int receiverId, CancellationToken ct = default);
    Task<IEnumerable<FriendRequest>> GetReceivedPendingAsync(int receiverId, CancellationToken ct = default);
}
