using MediatR;
using Product.Application.Social.GetFeed;

namespace Product.Application.Social.GetGroupPosts;

public class GetGroupPostsRequest : IRequest<GetFeedResponse>
{
    public int GroupId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
