using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Admin.GetUsers;

public class GetUsersHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var paged = await userRepository.SearchAsync(request.Search, request.PageIndex, request.PageSize, cancellationToken);

        return new GetUsersResponse
        {
            TotalCount = paged.TotalCount,
            PageIndex = paged.PageIndex,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(u => new AdminUserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                DisplayName = u.DisplayName,
                Role = u.Role,
                IsActive = u.IsActive,
                LastLoginAt = u.LastLoginAt,
                CreatedAt = u.CreatedAt
            }).ToList()
        };
    }
}
