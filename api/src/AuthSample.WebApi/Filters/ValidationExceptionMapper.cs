using AuthSample.BusinessLogic.Common.Exceptions;
using Hellang.Middleware.ProblemDetails;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace AuthSample.WebApi.Filters;

public static class ValidationExceptionMapper
{
    /// <summary>
    /// Configure mapping of <see cref="ValidationException"/> to <see cref="Microsoft.AspNetCore.Mvc.ValidationProblemDetails"/>
    /// </summary>
    /// <param name="options"></param>
    public static void MapValidationException(this ProblemDetailsOptions options)
    {
        options.Map<ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            return factory.CreateValidationProblemDetails(ctx, ex.Errors);
        });
    }
}
