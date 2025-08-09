using System.Text.Json.Serialization;

namespace OrchestratorService.Dtos;

public class OrderResponse
{
    [JsonPropertyName("id")]
    public Guid OrderId { get; set; }
}
