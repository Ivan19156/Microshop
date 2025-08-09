namespace OrchestratorService.Dtos;

public record CreateOrderCommand(Guid UserId, List<OrderItemDto> Items);

