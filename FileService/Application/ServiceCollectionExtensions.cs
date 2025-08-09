using Azure.Storage.Blobs;
using FileService.Infrastructure.Abstractions;
using FileService.Infrastructure.Services;

namespace FileService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            return new BlobServiceClient(connectionString);
        });

        return services;
    }
}
