using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Shared;

namespace OpenTelemetry.SampleProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
     
        private readonly ILogger<WeatherForecastController> logger;
        private readonly IWeatherForecastService forecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService forecastService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return this.forecastService.Get();
        }
    }
}
