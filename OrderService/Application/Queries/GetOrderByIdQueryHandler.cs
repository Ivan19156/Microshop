using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using OrderService.Application.DTOs;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories.Interfaces;

namespace OrderService.Application.Orders.Queries;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly OrderDbContext _context;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;


    public GetOrderByIdQueryHandler(OrderDbContext context, IMapper mapper, IOrderRepository repository)
    {
        _context = context;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdWithItemsAsync(request.OrderId, cancellationToken);

        return order is null ? null : _mapper.Map<OrderDto>(order);
    }
}
