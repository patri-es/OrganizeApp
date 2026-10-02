using Microsoft.Extensions.DependencyInjection;

namespace OrganizeApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        return services;
    }
}