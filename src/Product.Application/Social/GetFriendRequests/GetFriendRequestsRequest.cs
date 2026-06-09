using MediatR;

namespace Product.Application.Social.GetFriendRequests;

public class GetFriendRequestsRequest : IRequest<GetFriendRequestsResponse>
{
    public int UserId { get; set; }
}
