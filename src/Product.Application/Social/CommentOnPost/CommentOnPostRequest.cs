using MediatR;

namespace Product.Application.Social.CommentOnPost;

public class CommentOnPostRequest : IRequest<CommentOnPostResponse>
{
    public int PostId { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
}
