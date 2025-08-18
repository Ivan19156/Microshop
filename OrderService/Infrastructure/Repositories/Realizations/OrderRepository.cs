using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories.Interfaces;

namespace OrderService.Infrastructure.Repositories.Realizations;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(OrderDbContext context, ILogger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Items) // якщо потрібно
            .ToListAsync();
    }

    //public async Task<Order?> GetByIdAsync(Guid id)
    //{
    //    return await _context.Orders
    //        .Include(o => o.Items) // якщо потрібно
    //        .FirstOrDefaultAsync(o => o.Id == id);
    //}

    public async Task<Guid> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Attempting to add order with Id: {OrderId}", order.Id);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Order added successfully with Id: {OrderId}", order.Id);
        return order.Id;
    }



    public async Task DeleteAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }
    public async Task<List<Order>> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync(cancellationToken);
    }
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.FindAsync([id], cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
