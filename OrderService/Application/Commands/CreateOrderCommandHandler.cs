using AutoMapper;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Dtos;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories.Interfaces;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        var orderItems = _mapper.Map<List<OrderItem>>(request.Items);

        // 🔧 Встановлюємо зв'язок
        foreach (var item in orderItems)
        {
            item.OrderId = orderId; // або item.Order = order;
        }

        var order = new Order
        {
            Id = orderId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            Items = orderItems
        };

        return await _repository.AddAsync(order, cancellationToken);
    }
}
