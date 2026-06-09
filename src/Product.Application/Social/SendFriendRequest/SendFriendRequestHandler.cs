using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Social.SendFriendRequest;

public class SendFriendRequestHandler(
        IFriendRequestRepository requestRepository,
        IFriendshipRepository friendshipRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<SendFriendRequestRequest, SendFriendRequestResponse>
{
    public async Task<SendFriendRequestResponse> Handle(SendFriendRequestRequest request, CancellationToken cancellationToken)
    {
        if (request.SenderId == request.ReceiverId)
            throw new InvalidOperationException("Cannot send friend request to yourself.");

        if (await friendshipRepository.AreFriendsAsync(request.SenderId, request.ReceiverId, cancellationToken))
            throw new InvalidOperationException("Already friends.");

        var existing = await requestRepository.GetPendingAsync(request.SenderId, request.ReceiverId, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException("Friend request already sent.");

        var friendRequest = new Domain.Entities.FriendRequest
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId
        };

        await requestRepository.CreateAsync(friendRequest, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new SendFriendRequestResponse { RequestId = friendRequest.Id, Status = "Pending" };
    }
}
