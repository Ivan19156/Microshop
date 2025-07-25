using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Products.Commands;
using Application.Products.Queries;
using Application.Products.Dtos;
using Microsoft.Extensions.Logging;
using ProductDbContext;
using  Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;
    private readonly AppDbContext _context;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger, AppDbContext context)
    {
        _mediator = mediator;
        _logger = logger;
        _context = context;

    }
    [HttpPost] // This method handles HTTP POST requests
    [ProducesResponseType(StatusCodes.Status201Created)] // Specifies the success response type
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Specifies the error response type
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
       
        _logger.LogInformation("Attempting to create product with command: {@Command}", command);

        try
        {
            
            var productId = await _mediator.Send(command);

            _logger.LogInformation("Product created successfully. New product ID: {ProductId}", productId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, new { Id = productId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product with command: {@Command}", command);
            
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
        }
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }
    
    [HttpPost("test-create")]
public async Task<IActionResult> TestCreate()
{
    var product = new Product
    {
        Name = "Test Product",
        Price = 10.0m,
        Description = "Testing",
        CreatedAt = DateTime.UtcNow
    };

    _context.Products.Add(product);
    await _context.SaveChangesAsync();

    return Ok(new { product.Id });
}

}
