using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Social.GetFriends;

public class GetFriendsHandler(IFriendshipRepository friendshipRepository)
    : IRequestHandler<GetFriendsRequest, GetFriendsResponse>
{
    public async Task<GetFriendsResponse> Handle(GetFriendsRequest request, CancellationToken cancellationToken)
    {
        var friendships = await friendshipRepository.GetFriendshipsAsync(request.UserId, cancellationToken);

        var friends = friendships.Select(f =>
        {
            var friend = f.UserId == request.UserId ? f.Friend : f.User;
            return new FriendDto
            {
                UserId = friend.Id,
                Username = friend.Username,
                DisplayName = friend.DisplayName,
                AvatarUrl = friend.AvatarUrl
            };
        }).ToList();

        return new GetFriendsResponse { Friends = friends };
    }
}
