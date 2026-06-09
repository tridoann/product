using Product.Common.Models;
using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IGroupRepository : IRepository<Group, int>
{
    Task<PagedList<Group>> GetPublicGroupsAsync(int pageIndex, int pageSize, string? search = null, CancellationToken ct = default);
    Task<Group?> GetWithMembersAsync(int id, CancellationToken ct = default);
}
