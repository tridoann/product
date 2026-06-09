using Product.Common.Models;
using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IUserRepository : IRepository<User, int>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<PagedList<User>> SearchAsync(string? search, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<int> CountActiveAsync(CancellationToken ct = default);
}
