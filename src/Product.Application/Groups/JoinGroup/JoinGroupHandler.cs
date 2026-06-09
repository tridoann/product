using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Groups.JoinGroup;

public class JoinGroupHandler(IGroupRepository groupRepository, IGroupMemberRepository memberRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<JoinGroupRequest, JoinGroupResponse>
{
    public async Task<JoinGroupResponse> Handle(JoinGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetAsync(request.GroupId, cancellationToken)
            ?? throw new KeyNotFoundException("Group not found.");

        if (group.IsDeleted)
            throw new InvalidOperationException("Group no longer exists.");

        var existing = await memberRepository.GetAsync(request.GroupId, request.UserId, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException("Already a member.");

        await memberRepository.CreateAsync(new GroupMember
        {
            GroupId = request.GroupId,
            UserId = request.UserId,
            JoinedAt = DateTime.UtcNow
        }, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new JoinGroupResponse();
    }
}
