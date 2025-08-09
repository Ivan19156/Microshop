using MediatR;
using Application.Products.Dtos;

namespace Application.Products.Commands;

public record CreateProductCommand(Guid userId, CreateProductDto dto) : IRequest<Guid>;