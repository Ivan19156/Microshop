using OrchestratorService.Dtos;

namespace OrchestratorService.Clients;

public interface IProductServiceClient
{
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
}
