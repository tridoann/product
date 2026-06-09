using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class Post : BaseEntity<int>
{
    public int AuthorId { get; set; }
    public int? GroupId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    public MediaType MediaType { get; set; } = MediaType.None;
    public bool IsDeleted { get; set; } = false;

    public User Author { get; set; } = null!;
    public ICollection<PostLike> Likes { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
}
