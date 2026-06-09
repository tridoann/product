using Microsoft.EntityFrameworkCore;
using Product.Common.Models;
using Product.Domain.Entities;
using Product.Domain.Enums;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class GroupRepository(ProductDbContext dbContext)
    : BaseRepository<Group, int>(dbContext), IGroupRepository
{
    public async Task<PagedList<Group>> GetPublicGroupsAsync(int pageIndex, int pageSize, string? search = null, CancellationToken ct = default)
    {
        IQueryable<Group> source = _dbSet
            .Where(g => !g.IsDeleted && g.Privacy == GroupPrivacy.Public)
            .Include(g => g.CreatedBy)
            .Include(g => g.Members);

        if (!string.IsNullOrWhiteSpace(search))
            source = source.Where(g => g.Name.Contains(search));

        source = source.OrderBy(g => g.Name);

        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedList<Group> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public async Task<Group?> GetWithMembersAsync(int id, CancellationToken ct = default)
        => await _dbSet
            .Include(g => g.CreatedBy)
            .Include(g => g.Members).ThenInclude(m => m.User)
            .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted, ct);

    protected override IQueryable<Group> GetPagedCondition(string? searchQuery = null)
    {
        IQueryable<Group> source = _dbSet.Where(g => !g.IsDeleted).Include(g => g.CreatedBy);
        if (!string.IsNullOrWhiteSpace(searchQuery))
            source = source.Where(g => g.Name.Contains(searchQuery));
        return source.OrderBy(g => g.Name);
    }
}
