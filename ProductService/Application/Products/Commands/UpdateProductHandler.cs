using Application.Products.Commands;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Entities;
using ProductService.Infrastructure.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using AutoMapper;
using ProductService.Infrastructure.Repository;

namespace Application.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new Exception($"Product with ID {request.Id} not found");

        _mapper.Map(request.dto, product); 

        await _productRepository.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}

