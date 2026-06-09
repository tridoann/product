using MediatR;

namespace Product.Application.Groups.JoinGroup;

public class JoinGroupRequest : IRequest<JoinGroupResponse>
{
    public int GroupId { get; set; }
    public int UserId { get; set; }
}
