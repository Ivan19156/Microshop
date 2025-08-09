using MediatR;

public record DeleteOrderCommand(Guid OrderId) : IRequest<Unit>;
