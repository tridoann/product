using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface ICartRepository : IRepository<Cart, int>
{
    Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default);
}
