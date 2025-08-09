using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Products.Commands;
using Application.Products.Queries;
using Application.Products.Dtos;
using Microsoft.Extensions.Logging;
using Entities;
using ProductService.Infrastructure.Persistence;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProductService.Application.Products.Queries;

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
    [Authorize] 
    [HttpPost] 
    [ProducesResponseType(StatusCodes.Status201Created)] // Specifies the success response type
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Specifies the error response type
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Creating product for user with ID: {UserId}", userIdString);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            if (!Guid.TryParse(userIdString, out var userId))
                return BadRequest("Invalid user ID format.");

            _logger.LogInformation("Creating product for user with ID: {UserId}", userId);

            // Fix: Use the constructor of CreateProductCommand to pass required parameters  
            var updatedCommand = new CreateProductCommand(
                userId,
                command.dto
            );

            var productId = await _mediator.Send(updatedCommand);

            _logger.LogInformation("Product created successfully. New product ID: {ProductId}", productId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, new { Id = productId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product with command: {@Command}", command);

            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
        }
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
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
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received DELETE request for product with ID: {Id}", id);

        try
        {
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            _logger.LogInformation("Product with ID: {Id} successfully deleted", id);
            return NoContent(); // 204 No Content
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete product with ID: {Id}", id);
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
    {
        var updatedCommand = new UpdateProductCommand(id, command.dto);
        await _mediator.Send(updatedCommand);
        return NoContent();
    }
    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyProducts()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized();

        if (!Guid.TryParse(userIdString, out var userId))
            return BadRequest("Invalid user ID format.");

        var result = await _mediator.Send(new GetMyProductsQuery { UserId = userId });
        return Ok(result);
    }


}


