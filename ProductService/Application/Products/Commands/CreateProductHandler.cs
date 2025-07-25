using ProductDbContext;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;


namespace Application.Products.Commands;
public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(AppDbContext context, ILogger<CreateProductHandler> logger)
    {
        _context = context;
        _logger = logger;
        _logger.LogInformation("CreateProductHandler created");

    }

    
    public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateProductCommand for product: {ProductName}", command.Name);

       
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            ImageUrl = command.ImageUrl,
            CreatedAt = DateTime.UtcNow 
        };

        
        _context.Products.Add(product);

 
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product '{ProductName}' created successfully with ID: {ProductId}", product.Name, product.Id);


        return product.Id;
    }
}

