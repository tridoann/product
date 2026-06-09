namespace Product.Application.Social.CommentOnPost;

public class CommentOnPostResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
