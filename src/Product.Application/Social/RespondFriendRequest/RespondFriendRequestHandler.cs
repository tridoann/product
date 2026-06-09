using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Enums;
using Product.Domain.Repositories;

namespace Product.Application.Social.RespondFriendRequest;

public class RespondFriendRequestHandler(
        IFriendRequestRepository requestRepository,
        IFriendshipRepository friendshipRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<RespondFriendRequestRequest, RespondFriendRequestResponse>
{
    public async Task<RespondFriendRequestResponse> Handle(RespondFriendRequestRequest request, CancellationToken cancellationToken)
    {
        var friendRequest = await requestRepository.GetAsync(request.RequestId, cancellationToken)
            ?? throw new KeyNotFoundException("Friend request not found.");

        if (friendRequest.ReceiverId != request.ResponderId)
            throw new UnauthorizedAccessException("Not authorized to respond to this request.");

        if (friendRequest.Status != FriendRequestStatus.Pending)
            throw new InvalidOperationException("Request already responded to.");

        if (request.Accept)
        {
            friendRequest.Status = FriendRequestStatus.Accepted;
            var friendship = new Domain.Entities.Friendship
            {
                UserId = friendRequest.SenderId,
                FriendId = friendRequest.ReceiverId
            };
            await friendshipRepository.CreateAsync(friendship, cancellationToken);
        }
        else
        {
            friendRequest.Status = FriendRequestStatus.Rejected;
        }

        await unitOfWork.CommitAsync(cancellationToken);
        return new RespondFriendRequestResponse { Status = friendRequest.Status.ToString() };
    }
}
