using MediatR;

namespace Product.Application.Social.RespondFriendRequest;

public class RespondFriendRequestRequest : IRequest<RespondFriendRequestResponse>
{
    public int RequestId { get; set; }
    public int ResponderId { get; set; }
    public bool Accept { get; set; }
}
