using Entities;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Persistence;
using Application.Products.Dtos;
using AutoMapper;
using ProductService.Infrastructure.Repository;
using ProductService.Application.Products.Dtos;

namespace Application.Products.Queries;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        
        var productId = Guid.Parse(request.Id.ToString());
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }
}

