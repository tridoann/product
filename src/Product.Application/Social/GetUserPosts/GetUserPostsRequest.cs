using MediatR;

namespace Product.Application.Social.GetUserPosts;

public class GetUserPostsRequest : IRequest<GetUserPostsResponse>
{
    public int AuthorId { get; set; }
    public int RequesterId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
