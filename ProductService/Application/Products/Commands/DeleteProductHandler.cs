using MediatR;
using Microsoft.EntityFrameworkCore;
using Entities;
using ProductService.Infrastructure.Persistence;
using ProductService.Infrastructure.Repository;

namespace Application.Products.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
            throw new Exception($"Product with ID {request.Id} not found");

        await _productRepository.DeleteAsync(product, cancellationToken);

        return Unit.Value;
    }
}
