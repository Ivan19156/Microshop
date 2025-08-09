namespace OrchestratorService.Clients;

using OrchestratorService.Dtos;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class AuthServiceClient : IAuthServiceClient
{
    private readonly HttpClient _httpClient;

    public AuthServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/auth/{userId}");
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}

