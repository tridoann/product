using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Product.Infrastructure.Database;
using Product.IntegrationTest.Configurations;
using Xunit;

namespace Product.IntegrationTests;

public abstract class BaseContainerTest<T> : IClassFixture<WebApplicationFactory<T>> where T : class
{
    protected HttpClient? HttpClient;
    protected IServiceScopeFactory? ServiceScopeFactory;
    protected IServiceProvider? ServiceProvider;
    protected readonly ContainerFixture Container;
    protected IConfiguration? Configuration;
    private WebApplicationFactory<T> _factory;

    protected BaseContainerTest(ContainerFixture containerFixture,
            WebApplicationFactory<T> factory,
            bool autoInitializeResource = true)
        : base()
    {
        Container = containerFixture;
        _factory = factory;
        if (autoInitializeResource)
        {
            Initialize();
        }
    }
    protected void Initialize()
    {
        _factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("IntegrationTest");
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                //var env = hostingContext.HostingEnvironment;
                configBuilder.AddJsonFile("appsettings.json", false); // require the applications.json file
                                                                      //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", false); // require the applications.IntegrationTest.json file
                                                                      //configBuilder.AddEnvironmentVariables();
                configBuilder.AddContainerConfiguration(Container);
            });
            builder.ConfigureServices((context, serviceCollection) =>
            {
                // configure service here
                OverrideContainerVariables(serviceCollection);
                ConfigureServices(context.Configuration, serviceCollection);

            });
        });
        HttpClient = _factory!.CreateClient();
        Configuration = _factory.Services.GetRequiredService<IConfiguration>() ??
                                throw new ArgumentNullException(nameof(IConfiguration), (System.Exception?)null);

        ServiceProvider = _factory.Services ??
                                throw new ArgumentNullException(nameof(IServiceProvider), (System.Exception?)null);

        ServiceScopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>() ??
                                      throw new ArgumentNullException(nameof(IServiceScopeFactory), (System.Exception?)null);

    }

    protected virtual void ConfigureServices(IConfiguration configuration, IServiceCollection serviceCollection) { }

    private void OverrideContainerVariables(IServiceCollection serviceCollection)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        var sqlConnectionString = @$"Server=localhost,{Container.MsSql.GetMappedPublicPort(ContainerFixture.MsSqlPort)};Database=ProductDb;User Id=sa;Password={ContainerFixture.MsSqlPassword};Persist Security Info=False;Encrypt=False";
        optionsBuilder.UseSqlServer(sqlConnectionString, sqlOptions => sqlOptions.CommandTimeout(3600));
        serviceCollection.Replace(ServiceDescriptor.Scoped(provider => new ProductDbContext(optionsBuilder.Options)));

        // Configure Redis cache
        var redisConnectionString = @$"localhost:{Container.Redis.GetMappedPublicPort(ContainerFixture.RedisPort)}, password={ContainerFixture.RedisPassword}";
        var redisOptions = new RedisCacheOptions
        {
            Configuration = redisConnectionString,
            InstanceName = "CustomInstance"
        };

        serviceCollection.Replace(ServiceDescriptor.Singleton<IDistributedCache>(new RedisCache(Options.Create(redisOptions))));
    }
}