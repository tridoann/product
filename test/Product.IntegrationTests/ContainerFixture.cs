using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.MsSql;
using Testcontainers.Redis;
using Xunit;

namespace Product.IntegrationTests;

public abstract class ContainerFixture : IAsyncLifetime
{

    public const string MsSqlUser = "sa";
    public const string MsSqlPassword = "sqlserverintegrationtest@1234!";
    public const ushort MsSqlPort = 1433;
    public const string MsSqlServerImage = "mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04";

    public const string RedisPassword = "redistest@1234!";
    internal const ushort RedisPort = 6379;
    public const string RedisImage = "redis:latest";

    public const ushort HttpPort = 8080;


    public IContainer MsSql { get; }
    public RedisContainer Redis { get; }
    protected INetwork Network { get; }

    public Dictionary<string, string> CacheSettings { get; } = [];

    protected ContainerFixture()
    {
        // Initialize any resources needed for the tests here
        Network = new NetworkBuilder()
            .WithName(Guid.NewGuid().ToString("N"))
            .Build();

        var msSqlHost = Guid.NewGuid().ToString("N");
        var redisHost = Guid.NewGuid().ToString("N");

        MsSql = new MsSqlBuilder()
            .WithImage(MsSqlServerImage)
            .WithName(msSqlHost)
            .WithPassword(MsSqlPassword)
            .WithNetwork(Network)
            .WithPortBinding(MsSqlPort, true)
            .Build();

        Redis = new RedisBuilder()
            .WithImage(RedisImage)
            .WithName(redisHost)
            .WithCommand($"--requirepass {RedisPassword}")
            .WithPortBinding(RedisPort, true)
            .WithNetwork(Network)
            .Build();
            
        CacheSettings = new Dictionary<string, string>
        {
            {
                "Redis__Host", redisHost
            },
            {
                "Redis__Ssl", "false"
            },
            {
                "Redis__Password", RedisPassword
            },
            {
                "Redis__Database", "0"
            }
        };
    }
    public async Task InitializeAsync()
    {
        await Network.CreateAsync();
        await MsSql.StartAsync();
        await Redis.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await MsSql.DisposeAsync();
        await Redis.DisposeAsync();
        await Network.DisposeAsync();
    }

    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        await InitializeAsync();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        // Clean up resources after tests are done
        await DisposeAsync();
    }
}


public static class ContainerExtension
{
    public static ContainerBuilder AddEnvironmentVariables(this ContainerBuilder builder,
        ContainerFixture containerFixture)
    {
        return builder
            .WithEnvironment(containerFixture.CacheSettings);
    }
}