using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface ICartItemRepository : IRepository<CartItem, int>
{
    Task<CartItem?> GetAsync(int cartId, int productId, CancellationToken ct = default);
}
