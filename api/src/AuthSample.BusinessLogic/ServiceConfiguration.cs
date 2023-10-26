using System.Reflection;
using AuthSample.BusinessLogic.Common.RequestPipeline;
using AuthSample.Core;
using AuthSample.Core.TodoItemAggregate.Handlers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSample.BusinessLogic;

public static class ServiceConfiguration
{
    /// <summary>
    /// Configure dependency injection for interfaces implemented in the BusinessLogic project
    /// </summary>
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        var businessLogicAssembly = Assembly.GetExecutingAssembly();
        var coreAssembly = typeof(CoreServiceConfiguration).Assembly;

        services.AddValidatorsFromAssembly(businessLogicAssembly);

        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(
                businessLogicAssembly,
                coreAssembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}