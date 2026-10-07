using Microsoft.Extensions.DependencyInjection;
using OrganizeApp.Application.Category;
using OrganizeApp.Application.Category.Interfaces;
using OrganizeApp.Application.Product;
using OrganizeApp.Application.Product.Interfaces;

namespace OrganizeApp.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryUseCases, CategoryUseCases>();
        services.AddScoped<IProductUseCases, ProductUseCases>();

        return services;
    }
    //public static IServiceCollection AddApplication(
    //    this IServiceCollection services)
    //{
    //    services.AddScoped<IProductUseCases>();
    //    services.AddScoped<ICategoryUseCases>();
    //    // Registrar casos de uso para inyección por DI. Scoped para servicios que usan DbContext (por petición HTTP).
    //    //services.AddScoped<CategoryUseCases>();
    //    // Aquí podrían registrarse validadores, mappers y otros servicios de la capa Application.
    //    // ej: services.AddScoped<IProductValidator, ProductValidator>();
    //    return services;
    //}
}