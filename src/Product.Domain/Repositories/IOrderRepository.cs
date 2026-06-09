using Product.Common.Models;
using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IOrderRepository : IRepository<Order, int>
{
    Task<PagedList<Order>> GetByUserAsync(int userId, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<PagedList<Order>> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct = default);
    Task<Order?> GetWithItemsAsync(int id, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
}
