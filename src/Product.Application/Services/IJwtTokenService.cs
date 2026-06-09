using Product.Domain.Entities;

namespace Product.Application.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
