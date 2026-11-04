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
}