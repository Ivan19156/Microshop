namespace OrderService.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProductId { get; set; } // Ідентифікатор товару
    public string ProductName { get; set; } = null!; // Назва товару (на момент замовлення)

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Ціна за одиницю (на момент замовлення)

    public Guid OrderId { get; set; } // Foreign key
    public Order Order { get; set; } = null!;
}
