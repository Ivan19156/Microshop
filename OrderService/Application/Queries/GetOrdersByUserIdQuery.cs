using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries;

public class GetOrdersByUserIdQuery : IRequest<List<OrderDto>>
{
    public Guid UserId { get; set; }

    public GetOrdersByUserIdQuery(Guid userId)
    {
        UserId = userId;
    }
}

