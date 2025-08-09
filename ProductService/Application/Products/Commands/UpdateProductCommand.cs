using Application.Products.Dtos;
using MediatR;
using ProductService.Application.Products.Dtos;

namespace Application.Products.Commands;

public record UpdateProductCommand( Guid Id ,UpdateProductDto dto) : IRequest<Unit>;
