using Entities;
using MediatR;
using Entities;

namespace Application.Products.Queries;

public record GetAllProductsQuery() : IRequest<List<Product>>;
