using AuthSample.BusinessLogic;
using AuthSample.Core;
using AuthSample.Infrastructure;
using AuthSample.WebApi;
using AuthSample.WebApi.Logging;
using Hellang.Middleware.ProblemDetails;
using Serilog;
using Serilog.Events;

// First stage logger config
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

// Register application services
builder.Services
    .AddCoreServices()
    .AddBusinessLogicServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWebApiServices(builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging(options => options.EnrichDiagnosticContext = DiagnosticContextEnricher.EnrichFromRequest);

app.UseAuthorization();

app.UseProblemDetails();
app.MapControllers();

app.Run();
