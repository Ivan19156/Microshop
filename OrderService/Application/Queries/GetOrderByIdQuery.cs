using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries;

public class GetOrderByIdQuery : IRequest<OrderDto>
{
    public Guid OrderId { get; set; }

    public GetOrderByIdQuery(Guid orderId)
    {
        OrderId = orderId;
    }
}

