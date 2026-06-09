namespace Product.Domain.Entities;

public class PostLike : BaseEntity<int>
{
    public int PostId { get; set; }
    public int UserId { get; set; }

    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}
