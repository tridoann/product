using Product.Domain.Enums;

namespace Product.Application.Social.CreatePost;

public class CreatePostResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    public MediaType MediaType { get; set; }
    public DateTime CreatedAt { get; set; }
}
