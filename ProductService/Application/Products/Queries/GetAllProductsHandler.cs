using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductDbContext;
using Entities;


namespace Application.Products.Queries;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
{
    private readonly AppDbContext _context;

    public GetAllProductsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Database: {_context.Database.GetDbConnection().ConnectionString}");

        return await _context.Products.ToListAsync(cancellationToken);
    }
}
