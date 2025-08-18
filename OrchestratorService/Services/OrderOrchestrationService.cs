using OrchestratorService.Clients;
using OrchestratorService.Dtos;

namespace OrchestratorService.Services;

public class OrderOrchestrationService
{
    private readonly IAuthServiceClient _authClient;
    private readonly IProductServiceClient _productClient;
    private readonly IOrderServiceClient _orderClient;

    public OrderOrchestrationService(
        IAuthServiceClient authClient,
        IProductServiceClient productClient,
        IOrderServiceClient orderClient)
    {
        _authClient = authClient;
        _productClient = productClient;
        _orderClient = orderClient;
    }
    public async Task<Guid?> CreateOrderAsync(Guid userId, CreateOrderRequest request)
    {
        var user = await _authClient.GetUserByIdAsync(userId);
        if (user is null)
            throw new ArgumentException("User not found");

        var items = new List<OrderItemDto>();

        foreach (var item in request.Items)
        {
            var product = await _productClient.GetProductByIdAsync(item.ProductId);
            if (product is null)
                throw new ArgumentException($"Product {item.ProductId} not found");

            if (product.Stock < item.Quantity)
                throw new InvalidOperationException($"Not enough stock for product {product.Name}");

            items.Add(new OrderItemDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        var command = new CreateOrderCommand(
            UserId: userId,
            CustomerEmail: user.Email,      // нове поле
            CustomerPhone: user.Phone,      // нове поле
            Items: items
        );

        var orderId = await _orderClient.CreateOrderAsync(command);
        if (orderId == null)
            throw new InvalidOperationException("Failed to create order");

        return orderId;
    }


}

