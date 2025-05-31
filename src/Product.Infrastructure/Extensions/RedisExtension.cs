using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Infrastructure.Extensions;

public static class RedisExtension
{
    public static IServiceCollection AddStackExchangeRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "SampleInstance";
        });

        return services;
    }
}