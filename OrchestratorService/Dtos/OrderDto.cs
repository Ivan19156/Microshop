namespace OrchestratorService.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = default!;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}

