using MediatR;
using OrderService.Application.Dtos;

namespace OrderService.Application.Commands;
public record CreateOrderCommand(Guid UserId, List<OrderItemDto> Items) : IRequest<Guid>;
