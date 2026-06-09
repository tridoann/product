using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Social.CreatePost;

public class CreatePostHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePostRequest, CreatePostResponse>
{
    public async Task<CreatePostResponse> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var post = new Domain.Entities.Post
        {
            AuthorId = request.AuthorId,
            GroupId = request.GroupId,
            Content = request.Content,
            MediaUrl = request.MediaUrl,
            MediaType = request.MediaType
        };

        await postRepository.CreateAsync(post, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreatePostResponse
        {
            Id = post.Id,
            Content = post.Content,
            MediaUrl = post.MediaUrl,
            MediaType = post.MediaType,
            CreatedAt = post.CreatedAt
        };
    }
}
