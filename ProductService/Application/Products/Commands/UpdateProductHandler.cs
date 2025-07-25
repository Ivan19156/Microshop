using Application.Products.Commands;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductDbContext;
using Entities;

namespace Application.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdateProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
            throw new Exception($"Product with ID {request.Id} not found");

        product.Name = request.Product.Name;
        product.Description = request.Product.Description;
        product.Price = request.Product.Price;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
