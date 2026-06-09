using Product.Domain.Enums;

namespace Product.Application.Groups.CreateGroup;

public class CreateGroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public GroupPrivacy Privacy { get; set; }
    public DateTime CreatedAt { get; set; }
}
