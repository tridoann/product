using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Social.CommentOnPost;

public class CommentOnPostHandler(
        ICommentRepository commentRepository,
        IPostRepository postRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<CommentOnPostRequest, CommentOnPostResponse>
{
    public async Task<CommentOnPostResponse> Handle(CommentOnPostRequest request, CancellationToken cancellationToken)
    {
        _ = await postRepository.GetAsync(request.PostId, cancellationToken)
            ?? throw new KeyNotFoundException("Post not found.");

        var comment = new Domain.Entities.Comment
        {
            PostId = request.PostId,
            AuthorId = request.AuthorId,
            Content = request.Content
        };

        await commentRepository.CreateAsync(comment, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CommentOnPostResponse { Id = comment.Id, Content = comment.Content, CreatedAt = comment.CreatedAt };
    }
}
