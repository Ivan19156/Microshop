using Entities;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductDbContext;
using Entities;

namespace Application.Products.Queries;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly AppDbContext _context;

    public GetProductByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
    }
}
