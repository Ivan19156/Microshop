using MediatR;

namespace Application.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;