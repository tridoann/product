using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class GroupMemberRepository(ProductDbContext dbContext)
    : BaseRepository<GroupMember, int>(dbContext), IGroupMemberRepository
{
    public Task<GroupMember?> GetAsync(int groupId, int userId, CancellationToken ct = default)
        => _dbSet.FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == userId, ct);

    public Task<List<GroupMember>> GetGroupMembersAsync(int groupId, CancellationToken ct = default)
        => _dbSet.Where(m => m.GroupId == groupId).Include(m => m.User).ToListAsync(ct);

    protected override IQueryable<GroupMember> GetPagedCondition(string? searchQuery = null)
        => _dbSet;
}
