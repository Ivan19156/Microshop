using Entities;
using MediatR;
using Entities;

namespace Application.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<Product?>;
