using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IGroupMemberRepository : IRepository<GroupMember, int>
{
    Task<GroupMember?> GetAsync(int groupId, int userId, CancellationToken ct = default);
    Task<List<GroupMember>> GetGroupMembersAsync(int groupId, CancellationToken ct = default);
}
