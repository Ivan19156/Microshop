using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Order order, CancellationToken cancellationToken = default);

    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task DeleteAsync(Order order, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(); // якщо використовуєш UnitOfWork/DbContext
    Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default);

}
