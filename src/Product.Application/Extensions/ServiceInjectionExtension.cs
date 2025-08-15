using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Middlewares;
using Product.Application.Pipeline;
using Product.Application.Products.CreateProduct;
using Product.Application.Products.GetProducts;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;


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
        

        services.AddScoped<FluentValidation.IValidator<CreateProductRequest>, CreateProductRequestValidation>();
        services.AddScoped<FluentValidation.IValidator<GetProductsRequest>, GetProductsRequestValidation>();

        return services;
    }
}