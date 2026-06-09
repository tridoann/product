namespace Product.Domain.Entities;

public class Comment : BaseEntity<int>
{
    public int PostId { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

    public Post Post { get; set; } = null!;
    public User Author { get; set; } = null!;
}
