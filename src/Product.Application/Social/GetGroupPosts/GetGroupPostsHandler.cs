using MediatR;
using Product.Application.Social.GetFeed;
using Product.Domain.Repositories;

namespace Product.Application.Social.GetGroupPosts;

public class GetGroupPostsHandler(IPostRepository postRepository)
    : IRequestHandler<GetGroupPostsRequest, GetFeedResponse>
{
    public async Task<GetFeedResponse> Handle(GetGroupPostsRequest request, CancellationToken cancellationToken)
    {
        var paged = await postRepository.GetGroupPostsAsync(request.GroupId, request.PageIndex, request.PageSize, cancellationToken);

        return new GetFeedResponse
        {
            TotalCount = paged.TotalCount,
            PageIndex = paged.PageIndex,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(p => new PostDto
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                AuthorUsername = p.Author.Username,
                AuthorDisplayName = p.Author.DisplayName,
                AuthorAvatarUrl = p.Author.AvatarUrl,
                GroupId = p.GroupId,
                Content = p.Content,
                MediaUrl = p.MediaUrl,
                MediaType = p.MediaType,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count(c => !c.IsDeleted),
                RecentComments = p.Comments
                    .Where(c => !c.IsDeleted)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(3)
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        AuthorId = c.AuthorId,
                        AuthorDisplayName = c.Author.DisplayName,
                        AuthorAvatarUrl = c.Author.AvatarUrl,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt
                    }).ToList(),
                CreatedAt = p.CreatedAt
            }).ToList()
        };
    }
}
