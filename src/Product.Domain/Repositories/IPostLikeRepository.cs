using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IPostLikeRepository : IRepository<PostLike, int>
{
    Task<PostLike?> GetAsync(int postId, int userId, CancellationToken ct = default);
}
