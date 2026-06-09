using MediatR;

namespace Product.Application.Social.LikePost;

public class LikePostRequest : IRequest<LikePostResponse>
{
    public int PostId { get; set; }
    public int UserId { get; set; }
}
