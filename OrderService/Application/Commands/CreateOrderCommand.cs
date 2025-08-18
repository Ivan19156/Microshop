using MediatR;
using OrderService.Application.Dtos;

namespace OrderService.Application.Commands;
public record CreateOrderCommand(
    Guid UserId,
    string CustomerEmail,
    string? CustomerPhone,
    List<OrderItemDto> Items
) : IRequest<Guid>;
