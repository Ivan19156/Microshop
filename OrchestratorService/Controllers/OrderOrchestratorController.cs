namespace OrchestratorService.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchestratorService.Dtos;
using OrchestratorService.Services;
using System.Security.Claims;

[ApiController]
[Route("api/orders")]
public class OrderOrchestratorController : ControllerBase
{
    private readonly OrderOrchestrationService _orderOrchestrationService;
    private readonly ILogger<OrderOrchestratorController> _logger;

    public OrderOrchestratorController(OrderOrchestrationService orderOrchestrationService, ILogger<OrderOrchestratorController> logger)
    {
        _orderOrchestrationService = orderOrchestrationService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized("Invalid or missing user ID in token");

            var orderId = await _orderOrchestrationService.CreateOrderAsync(userId, request);
            return Ok(new { OrderId = orderId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Unexpected error", details = ex.Message });
        }
    }
}



