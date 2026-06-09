using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Product.Infrastructure.Extensions;

public static class PostgresExtension
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Database.ProductDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.CommandTimeout(3600)));

        return services;
    }
}
