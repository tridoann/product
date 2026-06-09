using Product.Common.Models;
using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IPostRepository : IRepository<Post, int>
{
    Task<PagedList<Post>> GetFeedAsync(int userId, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<PagedList<Post>> GetGroupPostsAsync(int groupId, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<PagedList<Post>> GetUserPostsAsync(int authorId, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<int> CountUserPostsAsync(int authorId, CancellationToken ct = default);
}
