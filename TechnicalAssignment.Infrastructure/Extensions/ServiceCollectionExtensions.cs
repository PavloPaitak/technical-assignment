using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using TechnicalAssignment.Infrastructure.HttpClients;

namespace TechnicalAssignment.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddItemApi(this IServiceCollection services, IConfiguration config)
    {
        var baseUrl = config["ItemApi:BaseUrl"] ?? throw new InvalidOperationException("Missing ItemApi:BaseUrl");

        var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        }));

        services
            .AddRefitClient<IItemApi>(refitSettings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

        return services;
    }
}
