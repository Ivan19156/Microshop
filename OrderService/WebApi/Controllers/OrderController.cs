using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using OrderService.Application.Orders.Queries;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            _logger.LogInformation("Received CreateOrderCommand request: {@Command}", command);
            var orderId = await _mediator.Send(command);
            _logger.LogInformation("Order created with Id: {OrderId}", orderId);
            return CreatedAtAction(nameof(GetById), new { id = orderId }, new { OrderId = orderId });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteOrderCommand(id));
            return NoContent();
        }

        [Authorize]
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders(CancellationToken cancellationToken)
        {
            // Отримання userId з токена
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized("Невірний або відсутній userId у токені.");

            // Надсилання CQRS-запиту
            var query = new GetOrdersByUserIdQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
