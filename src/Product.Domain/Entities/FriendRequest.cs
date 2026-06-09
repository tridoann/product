using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class FriendRequest : BaseEntity<int>
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;

    public User Sender { get; set; } = null!;
    public User Receiver { get; set; } = null!;
}
