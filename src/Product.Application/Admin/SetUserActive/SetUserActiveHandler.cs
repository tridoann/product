using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Admin.SetUserActive;

public class SetUserActiveHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<SetUserActiveRequest, SetUserActiveResponse>
{
    public async Task<SetUserActiveResponse> Handle(SetUserActiveRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        user.IsActive = request.IsActive;
        await unitOfWork.CommitAsync(cancellationToken);

        return new SetUserActiveResponse { UserId = user.Id, IsActive = user.IsActive };
    }
}
