using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Унікальний ідентифікатор замовлення
    public Guid UserId { get; set; } // Хто зробив замовлення

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public List<OrderItem> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(item => item.Quantity * item.UnitPrice);
    public string CustomerEmail { get; set; } = default!;
    public string? CustomerPhone { get; set; } = default!;
}
