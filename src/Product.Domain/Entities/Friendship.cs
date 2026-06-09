namespace Product.Domain.Entities;

public class Friendship : BaseEntity<int>
{
    public int UserId { get; set; }
    public int FriendId { get; set; }

    public User User { get; set; } = null!;
    public User Friend { get; set; } = null!;
}
