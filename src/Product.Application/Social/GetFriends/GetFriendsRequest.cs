using MediatR;

namespace Product.Application.Social.GetFriends;

public class GetFriendsRequest : IRequest<GetFriendsResponse>
{
    public int UserId { get; set; }
}
