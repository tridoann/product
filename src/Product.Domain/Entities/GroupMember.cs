using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class GroupMember : BaseEntity<int>
{
    public int GroupId { get; set; }
    public int UserId { get; set; }
    public GroupMemberRole Role { get; set; } = GroupMemberRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Group Group { get; set; } = null!;
    public User User { get; set; } = null!;
}
