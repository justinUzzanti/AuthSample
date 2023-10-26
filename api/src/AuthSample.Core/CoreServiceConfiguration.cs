using Microsoft.Extensions.DependencyInjection;

namespace AuthSample.Core;

public static class CoreServiceConfiguration
{
    /// <summary>
    /// Configure dependency injection for interfaces implemented in the Core project
    /// </summary>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Register Domain Services here

        return services;
    }
}