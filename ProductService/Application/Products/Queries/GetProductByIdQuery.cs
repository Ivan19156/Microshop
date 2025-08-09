using Entities;
using MediatR;
using Application.Common.Abstractions;
using Application.Products.Dtos;
using ProductService.Application.Products.Dtos;

namespace Application.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto?>;

