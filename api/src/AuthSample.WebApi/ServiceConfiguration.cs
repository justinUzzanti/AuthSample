using AuthSample.Core.Common.Exceptions;
using AuthSample.WebApi.Filters;
using Hellang.Middleware.ProblemDetails;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace AuthSample.WebApi;

public static class ServiceConfiguration
{
    /// <summary>
    /// Configure dependency injection for interfaces implemented in the WebApi project
    /// </summary>
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services
            // Configure Problem Details middleware
            // For more information, see:
            // https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-6.0#custom-middleware-to-handle-exceptions
            // https://github.com/khellang/Middleware
            // https://datatracker.ietf.org/doc/html/rfc7807
            .AddProblemDetails(options => ConfigureProblemDetails(options, environment))
            .AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(x => x.FullName?.Replace("+", ".")); // Needed because of our nested classes in MediatR requests
        });

        return services;
    }

    private static void ConfigureProblemDetails(ProblemDetailsOptions problemDetailsOptions, IWebHostEnvironment environment)
    {
        // Only include exception details in a development environment. This is the
        // default behavior but put here for clarity
        problemDetailsOptions.IncludeExceptionDetails = (ctx, ex) => environment.IsDevelopment();

        // Map FluentValidation errors from the MediatR pipeline
        problemDetailsOptions.MapValidationException();

        // Map other exception types to HTTP status codes
        problemDetailsOptions.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
    }
}
