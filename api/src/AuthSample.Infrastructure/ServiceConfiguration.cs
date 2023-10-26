using Centeva.DomainModeling;
using Centeva.DomainModeling.Interfaces;
using AuthSample.Core.Interfaces;
using AuthSample.Infrastructure.Persistence;
using AuthSample.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSample.Infrastructure;

public static class ServiceConfiguration
{
    /// <summary>
    /// Configure dependency injection for interfaces implemented in the Infrastructure project
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention();
        });
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddTransient<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}