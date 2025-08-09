using AutoMapper;
using Azure.Core;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using ProductService.Infrastructure.Persistence;
using ProductService.Infrastructure.Repository;


namespace Application.Products.Commands;
public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository productRepository, ILogger<CreateProductHandler> logger, IMapper mapper)
    {
        _productRepository = productRepository;
        _logger = logger;
        _mapper = mapper;
        _logger.LogInformation("CreateProductHandler created");
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(command.dto);
        product.UserId = command.userId;
        _logger.LogInformation("Creating product for user with ID: {UserId}", command.userId);
        _logger.LogInformation("Creating product for user with ID: {UserId}", product.Id);
        await _productRepository.AddAsync(product, cancellationToken);

        _logger.LogInformation("Product '{ProductName}' created successfully with ID: {ProductId}", product.Name, product.Id);

        return product.Id;
    }
}
