using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Social.GetFriendRequests;

public class GetFriendRequestsHandler(IFriendRequestRepository requestRepository)
    : IRequestHandler<GetFriendRequestsRequest, GetFriendRequestsResponse>
{
    public async Task<GetFriendRequestsResponse> Handle(GetFriendRequestsRequest request, CancellationToken cancellationToken)
    {
        var requests = await requestRepository.GetReceivedPendingAsync(request.UserId, cancellationToken);

        return new GetFriendRequestsResponse
        {
            Requests = requests.Select(r => new FriendRequestDto
            {
                Id = r.Id,
                SenderId = r.SenderId,
                SenderUsername = r.Sender.Username,
                SenderDisplayName = r.Sender.DisplayName,
                SenderAvatarUrl = r.Sender.AvatarUrl,
                SentAt = r.CreatedAt
            }).ToList()
        };
    }
}
