using Product.Domain.Enums;

namespace Product.Application.Groups.GetGroup;

public class GetGroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
    public GroupPrivacy Privacy { get; set; }
    public int MemberCount { get; set; }
    public bool IsCurrentUserMember { get; set; }
    public GroupMemberRole? CurrentUserRole { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GroupMemberDto> Members { get; set; } = [];
}

public class GroupMemberDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public GroupMemberRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
}
