using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthSample.BusinessLogic.WeatherForecasts;

public class GetWeatherForecast
{
    public class Query : IRequest<List<Result>>
    {
        // No params
    }

    public class Result
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; } = String.Empty;
    }

    public class Handler : IRequestHandler<Query, List<Result>>
    {
        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        public Task<List<Result>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sample log message, here are the possible forecast summaries: {summaries}",
                (object)Summaries);

            var result = Enumerable.Range(1, 5).Select(index => new Result
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();

            return Task.FromResult(result);
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }
}
