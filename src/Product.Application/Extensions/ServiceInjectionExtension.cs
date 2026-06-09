using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Middlewares;
using Product.Application.Pipeline;

namespace Product.Application.Extensions;

public static class ServiceInjectionExtension
{
    public static object AddServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        // Register all validators in this assembly automatically
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}