using MediatR;
using OrderService.Domain.Enums;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus NewStatus) : IRequest<Unit>;
