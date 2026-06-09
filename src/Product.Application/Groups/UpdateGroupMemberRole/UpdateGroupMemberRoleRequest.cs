using MediatR;
using Product.Domain.Enums;

namespace Product.Application.Groups.UpdateGroupMemberRole;

public class UpdateGroupMemberRoleRequest : IRequest<UpdateGroupMemberRoleResponse>
{
    public int GroupId { get; set; }
    public int RequesterId { get; set; }
    public int TargetUserId { get; set; }
    public GroupMemberRole NewRole { get; set; }
}
