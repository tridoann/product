using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Middlewares;


namespace Product.Application.Extensions;

public static class ServiceInjectionExtension
{
    public static object AddServices(this IServiceCollection services)
    {

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            var profiles = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(Profile)
                .IsAssignableFrom(x));

            foreach (var profile in profiles)
            {
                if (profile.GetConstructor(Type.EmptyTypes) is not null
                    && !profile.IsAbstract
                    && !profile.IsInterface)
                {
                    cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
                }
            }
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}