using MediatR;

namespace Product.Application.Groups.LeaveGroup;

public class LeaveGroupRequest : IRequest<LeaveGroupResponse>
{
    public int GroupId { get; set; }
    public int UserId { get; set; }
}
