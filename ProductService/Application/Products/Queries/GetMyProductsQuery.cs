namespace ProductService.Application.Products.Queries;

using MediatR;
using ProductService.Application.Products.Dtos;

public class GetMyProductsQuery : IRequest<List<ProductDto>>
{
    public Guid UserId { get; set; } = default!;
}

