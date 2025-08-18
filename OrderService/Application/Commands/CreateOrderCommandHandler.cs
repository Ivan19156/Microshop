using AutoMapper;
using MassTransit;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Dtos;
using OrderService.Domain.Entities;
using Microshop.Contracts.Events;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories.Interfaces;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(
        IOrderRepository repository,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Start handling CreateOrderCommand for UserId: {UserId}, Email: {Email}", request.UserId, request.CustomerEmail);

            var orderId = Guid.NewGuid();

            var order = _mapper.Map<Order>(request);
            order.Id = orderId;
            order.CreatedAt = DateTime.UtcNow;

            foreach (var item in order.Items)
            {
                item.OrderId = orderId;
                _logger.LogInformation("Order item: ProductId={ProductId}, Quantity={Quantity}, UnitPrice={UnitPrice}", item.ProductId, item.Quantity, item.UnitPrice);
            }

            _logger.LogInformation("Saving order with Id: {OrderId}", orderId);
            await _repository.AddAsync(order, cancellationToken);
            _logger.LogInformation("Order saved successfully with Id: {OrderId}", orderId);

            var orderCreatedEvent = _mapper.Map<OrderCreatedEvent>(order);
            _logger.LogInformation("Publishing OrderCreatedEvent for OrderId: {OrderId}", orderId);
            await _publishEndpoint.Publish(orderCreatedEvent, cancellationToken);
            _logger.LogInformation("OrderCreatedEvent published successfully for OrderId: {OrderId}", orderId);

            return orderId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating order for UserId: {UserId}, Email: {Email}", request.UserId, request.CustomerEmail);
            throw; // пробросити далі, щоб контролер міг повернути помилку
        }
    }
}
