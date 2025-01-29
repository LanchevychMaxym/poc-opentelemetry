using Microsoft.Extensions.Logging;
using OpenTelemetry.Shared;
using OpenTelemetry.Shared.Traceability;

namespace OpenTelemetry.Service
{
    public class WeatherForecastService : Traceable<WeatherForecastService>, IWeatherForecastService
    {
        private readonly IWeatherRepository weatherRepository;
        private readonly ILogger<WeatherForecastService> logger;
        public WeatherForecastService(IWeatherRepository repository, ILogger<WeatherForecastService> logger) 
        {
            this.weatherRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IEnumerable<WeatherForecast> Get()
        {
            return this.TraceAction(() =>
            {
                this.logger.LogInformation("Getting forecast.");
                var forecast = this.weatherRepository.Get();
                // structured logging
                this.logger.LogInformation("Found: {@forecast}", forecast);
                return forecast;
            });
        }
    }
}
