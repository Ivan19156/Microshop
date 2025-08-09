using MediatR;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories.Interfaces;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderRepository _repository;

    public UpdateOrderStatusCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");

        order.Status = request.NewStatus;

        await _repository.UpdateAsync(order, cancellationToken);

        return Unit.Value;
    }
}
