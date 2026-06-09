using Product.Application.Social.GetFeed;

namespace Product.Application.Social.GetUserPosts;

public class GetUserPostsResponse
{
    public List<PostDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
