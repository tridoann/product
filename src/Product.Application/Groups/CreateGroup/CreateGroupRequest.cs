using MediatR;
using Product.Domain.Enums;

namespace Product.Application.Groups.CreateGroup;

public class CreateGroupRequest : IRequest<CreateGroupResponse>
{
    public int CreatedById { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GroupPrivacy Privacy { get; set; } = GroupPrivacy.Public;
}
