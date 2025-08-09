using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.DTOs;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories.Interfaces;

namespace OrderService.Application.Orders.Queries;

public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, List<OrderDto>>
{
    private readonly OrderDbContext _context;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;

    public GetOrdersByUserIdQueryHandler(OrderDbContext context, IMapper mapper, IOrderRepository repository)
    {
        _context = context;
        _mapper = mapper;
        _repository = repository;   
    }

    public async Task<List<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetByUserIdWithItemsAsync(request.UserId, cancellationToken);

        return _mapper.Map<List<OrderDto>>(orders);

    }
}

