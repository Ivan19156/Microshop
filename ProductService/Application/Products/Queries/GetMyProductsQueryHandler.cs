namespace ProductService.Application.Products.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Products.Dtos;


using ProductService.Infrastructure.Repository;

public class GetMyProductsQueryHandler : IRequestHandler<GetMyProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;

    public GetMyProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<List<ProductDto>> Handle(GetMyProductsQuery request, CancellationToken cancellationToken)
    {
        
        return _repository.GetProductsByUserIdAsync(request.UserId, cancellationToken);
    }
}


