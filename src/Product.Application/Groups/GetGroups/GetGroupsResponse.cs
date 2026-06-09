using Product.Domain.Enums;

namespace Product.Application.Groups.GetGroups;

public class GetGroupsResponse
{
    public List<GroupDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }
    public GroupPrivacy Privacy { get; set; }
    public int MemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
