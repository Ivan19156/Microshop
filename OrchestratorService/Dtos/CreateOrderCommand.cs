namespace OrchestratorService.Dtos;



public record CreateOrderCommand(
    Guid UserId,
    string CustomerEmail,
    string? CustomerPhone,
    List<OrderItemDto> Items
);
