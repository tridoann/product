using Microsoft.Extensions.Configuration;
using Product.IntegrationTests;

namespace Product.IntegrationTest.Configurations;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddContainerConfiguration(
        this IConfigurationBuilder builder, ContainerFixture containerFixture)
    {
        return builder.Add(new ContainerConfigurationSource(containerFixture));
    }
}

public class ContainerConfigurationSource : IConfigurationSource
{
    private readonly ContainerFixture _container;

    public ContainerConfigurationSource(ContainerFixture container)
    {
        _container = container;
    }
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new ContainerConfigurationProvider(_container);
    }
}


public class ContainerConfigurationProvider : ConfigurationProvider
{
    private readonly ContainerFixture _container;

    public ContainerConfigurationProvider(ContainerFixture container)
    {
        _container = container;
    }
    public override void Load()
    {
        // adding redis
        Data = new Dictionary<string, string?>
        {
            // _container.BaseSettings.ToConfiguration(),
            _container.CacheSettings.ToConfiguration(),
        };

        Data["Redis:Host"] = _container.Redis.Hostname;
        Data["Redis:Port"] = _container.Redis.GetMappedPublicPort(ContainerFixture.RedisPort).ToString();

        Data["Database:Host"] = _container.MsSql.Hostname;
        Data["Database:Port"] = _container.MsSql.GetMappedPublicPort(ContainerFixture.MsSqlPort).ToString();

        
        // loggin section
        Data["Logging:LogLevel:EWS"] = "Debug";
        Data["Logging:LogLevel:Default"] = "Warning";
        // MassTransit logging level
        Data["Logging:LogLevel:OpenIddict"] = "Warning";
        Data["Logging:LogLevel:MassTransit"] = "Warning";
        Data["Logging:LogLevel:Microsoft"] = "Warning";
        Data["Logging:LogLevel:System.Net"] = "Warning";
        Data["Logging:LogLevel:Microsoft.Hosting.Lifetime"] = "Warning";
        Data["Logging:LogLevel:Microsoft.AspNetCore"] = "Warning";
        Data["Logging:LogLevel:Microsoft.EntityFrameworkCore"] = "Warning";
        Data["Logging:LogLevel:System.Net.Http.HttpClient.OpenIddict.Validation.SystemNetHttp"] = "Warning";

    }
}
public static class ConfigurationExtensions
{
    public static IEnumerable<KeyValuePair<string, string>> ToConfiguration(this IReadOnlyDictionary<string, string> settings)
    {
        foreach (var setting in settings)
        {
            yield return new KeyValuePair<string, string>(setting.Key.Replace("__", ":"), setting.Value);
        }
    }
    public static void Add(this IDictionary<string, string?> data, IEnumerable<KeyValuePair<string, string>> pair)
    {
        foreach (var item in pair)
        {
            data.Add(item.Key, item.Value);
        }
    }
}