using MediatR;
using Cache;
namespace Application.Behaviors;
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cacheService;

    public CachingBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Генеруємо ключ кешу на основі типу запиту + його даних (якщо потрібно)
        var cacheKey = $"{typeof(TRequest).FullName}_{System.Text.Json.JsonSerializer.Serialize(request)}";

        var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey);
        if (cachedResponse != null)
        {
            return cachedResponse;
        }

        var response = await next();

        // Зберігаємо результат у кеш, наприклад на 5 хвилин
        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5));

        return response;
    }
}
