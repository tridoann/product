using MediatR;

namespace Product.Application.Social.DeletePost;

public class DeletePostRequest : IRequest<DeletePostResponse>
{
    public int PostId { get; set; }
    public int RequesterId { get; set; }
}
