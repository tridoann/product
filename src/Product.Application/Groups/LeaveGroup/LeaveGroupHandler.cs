using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Groups.LeaveGroup;

public class LeaveGroupHandler(IGroupMemberRepository memberRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<LeaveGroupRequest, LeaveGroupResponse>
{
    public async Task<LeaveGroupResponse> Handle(LeaveGroupRequest request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetAsync(request.GroupId, request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("Not a member of this group.");

        memberRepository.Remove(member);
        await unitOfWork.CommitAsync(cancellationToken);

        return new LeaveGroupResponse();
    }
}
