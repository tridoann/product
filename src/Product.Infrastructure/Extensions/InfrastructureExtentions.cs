using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Product.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer(configuration);
        services.AddStackExchangeRedisCache(configuration);

        return services;
    }
}