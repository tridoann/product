using MediatR;

namespace Product.Application.Social.SendFriendRequest;

public class SendFriendRequestRequest : IRequest<SendFriendRequestResponse>
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
}
