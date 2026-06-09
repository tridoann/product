using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IFriendshipRepository : IRepository<Friendship, int>
{
    Task<bool> AreFriendsAsync(int userId, int friendId, CancellationToken ct = default);
    Task<IEnumerable<Friendship>> GetFriendshipsAsync(int userId, CancellationToken ct = default);
}
