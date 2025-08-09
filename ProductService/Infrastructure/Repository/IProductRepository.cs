using Entities;
using ProductService.Application.Products.Dtos;

namespace ProductService.Infrastructure.Repository;

// Application/Abstractions/IProductRepository.cs
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Product product, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<ProductDto>> GetProductsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

