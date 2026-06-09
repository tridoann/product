using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class Group : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
    public GroupPrivacy Privacy { get; set; } = GroupPrivacy.Public;
    public int CreatedById { get; set; }
    public bool IsDeleted { get; set; }

    public User CreatedBy { get; set; } = null!;
    public ICollection<GroupMember> Members { get; set; } = [];
}
