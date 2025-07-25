using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class HealthCheckHandler : DelegatingHandler
{
    private readonly HttpClient _httpClient;

    public HealthCheckHandler()
    {
        _httpClient = new HttpClient();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var baseUri = $"{request.RequestUri.Scheme}://{request.RequestUri.Host}:{request.RequestUri.Port}";
        var healthUri = $"{baseUri}/health";

        try
        {
            var healthResponse = await _httpClient.GetAsync(healthUri, cancellationToken);
            if (!healthResponse.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    ReasonPhrase = $"Downstream service unhealthy at {healthUri}"
                };
            }
        }
        catch
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
            {
                ReasonPhrase = $"Downstream service unreachable at {healthUri}"
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
