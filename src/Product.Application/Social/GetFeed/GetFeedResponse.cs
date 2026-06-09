using Product.Domain.Enums;

namespace Product.Application.Social.GetFeed;

public class GetFeedResponse
{
    public List<PostDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage => PageIndex * PageSize < TotalCount;
}

public class PostDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public string AuthorDisplayName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }
    public int? GroupId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? MediaUrl { get; set; }
    public MediaType MediaType { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public List<CommentDto> RecentComments { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}

public class CommentDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string AuthorDisplayName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
