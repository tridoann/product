using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;
using Product.Infrastructure.Repositories;


namespace Product.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer(configuration);
        services.AddStackExchangeRedisCache(configuration);

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}