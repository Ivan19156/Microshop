using OrchestratorService.Dtos;

namespace OrchestratorService.Clients;

public interface IOrderServiceClient
{
    Task<Guid?> CreateOrderAsync(CreateOrderCommand command);
}

