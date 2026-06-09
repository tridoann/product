using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Enums;
using Product.Domain.Repositories;

namespace Product.Application.Groups.UpdateGroupMemberRole;

public class UpdateGroupMemberRoleHandler(IGroupMemberRepository memberRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateGroupMemberRoleRequest, UpdateGroupMemberRoleResponse>
{
    public async Task<UpdateGroupMemberRoleResponse> Handle(UpdateGroupMemberRoleRequest request, CancellationToken cancellationToken)
    {
        var requester = await memberRepository.GetAsync(request.GroupId, request.RequesterId, cancellationToken)
            ?? throw new UnauthorizedAccessException("Not a member of this group.");

        if (requester.Role != GroupMemberRole.Admin)
            throw new UnauthorizedAccessException("Only group admins can change member roles.");

        var target = await memberRepository.GetAsync(request.GroupId, request.TargetUserId, cancellationToken)
            ?? throw new KeyNotFoundException("Target user is not a member.");

        target.Role = request.NewRole;
        await unitOfWork.CommitAsync(cancellationToken);

        return new UpdateGroupMemberRoleResponse();
    }
}
