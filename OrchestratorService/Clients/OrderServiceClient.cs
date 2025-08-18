namespace OrchestratorService.Clients;

using OrchestratorService.Dtos;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class OrderServiceClient : IOrderServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderServiceClient> _logger;

    public OrderServiceClient(HttpClient httpClient, ILogger<OrderServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Guid?> CreateOrderAsync(CreateOrderCommand command)
    {
        var json = JsonSerializer.Serialize(command);
        _logger.LogInformation("JSON being sent to OrderService: {Json}", json);

        var response = await _httpClient.PostAsJsonAsync("/api/order", command);
        _logger.LogInformation("OrderService response: " + response);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
        _logger.LogInformation("OrderService response content: " + result);
        return result?.OrderId;
    }

}

