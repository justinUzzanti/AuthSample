using AuthSample.BusinessLogic.WeatherForecasts;
using Microsoft.AspNetCore.Mvc;

namespace AuthSample.WebApi.Controllers;

public class WeatherForecastController : ApiControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public Task<List<GetWeatherForecast.Result>> Get() => Mediator.Send(new GetWeatherForecast.Query());
}