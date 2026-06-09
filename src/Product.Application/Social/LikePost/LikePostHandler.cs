using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Social.LikePost;

public class LikePostHandler(
        IPostLikeRepository likeRepository,
        IPostRepository postRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<LikePostRequest, LikePostResponse>
{
    public async Task<LikePostResponse> Handle(LikePostRequest request, CancellationToken cancellationToken)
    {
        _ = await postRepository.GetAsync(request.PostId, cancellationToken)
            ?? throw new KeyNotFoundException("Post not found.");

        var existing = await likeRepository.GetAsync(request.PostId, request.UserId, cancellationToken);
        if (existing is not null)
        {
            likeRepository.Remove(existing);
            await unitOfWork.CommitAsync(cancellationToken);
            return new LikePostResponse { Liked = false };
        }

        await likeRepository.CreateAsync(new Domain.Entities.PostLike
        {
            PostId = request.PostId,
            UserId = request.UserId
        }, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return new LikePostResponse { Liked = true };
    }
}
