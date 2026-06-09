using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Social.DeletePost;

public class DeletePostHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePostRequest, DeletePostResponse>
{
    public async Task<DeletePostResponse> Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetAsync(request.PostId, cancellationToken)
            ?? throw new KeyNotFoundException("Post not found.");

        if (post.AuthorId != request.RequesterId)
            throw new UnauthorizedAccessException("Only the author can delete this post.");

        post.IsDeleted = true;
        await unitOfWork.CommitAsync(cancellationToken);

        return new DeletePostResponse();
    }
}
