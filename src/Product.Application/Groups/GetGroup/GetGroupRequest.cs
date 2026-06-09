using MediatR;

namespace Product.Application.Groups.GetGroup;

public class GetGroupRequest : IRequest<GetGroupResponse>
{
    public int GroupId { get; set; }
    public int RequesterId { get; set; }
}
