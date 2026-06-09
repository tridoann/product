using MediatR;
using Product.Domain.Enums;

namespace Product.Application.Social.CreatePost;

public class CreatePostRequest : IRequest<CreatePostResponse>
{
    public int AuthorId { get; set; }
    public int? GroupId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    public MediaType MediaType { get; set; } = MediaType.None;
}
