using Microsoft.EntityFrameworkCore;
using Product.Common.Models;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class UserRepository(ProductDbContext dbContext)
    : BaseRepository<User, int>(dbContext), IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => _dbSet.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(u => u.Email == email, cancellationToken);

    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(u => u.Username == username, cancellationToken);

    public async Task<PagedList<User>> SearchAsync(string? search, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        IQueryable<User> source = _dbSet;
        if (!string.IsNullOrWhiteSpace(search))
            source = source.Where(u => u.Username.Contains(search) || u.Email.Contains(search));
        source = source.OrderBy(u => u.Username);
        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedList<User> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public Task<int> CountAsync(CancellationToken ct = default) => _dbSet.CountAsync(ct);
    public Task<int> CountActiveAsync(CancellationToken ct = default) => _dbSet.CountAsync(u => u.IsActive, ct);

    protected override IQueryable<User> GetPagedCondition(string? searchQuery = null)
    {
        var source = GetDbSet().Where(u => u.IsActive);
        if (!string.IsNullOrWhiteSpace(searchQuery))
            source = source.Where(u => u.Username.Contains(searchQuery)
                || u.Email.Contains(searchQuery)
                || u.DisplayName.Contains(searchQuery));
        return source.OrderBy(u => u.Username);
    }
}
