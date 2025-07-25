using MediatR;
using Application.Products.Dtos;

namespace Application.Products.Commands;

public record CreateProductCommand(string Name, string Description, decimal Price, string? ImageUrl ) : IRequest<int>;