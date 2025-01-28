using OpenTelemetry.Shared;

namespace OpenTelemetry.Service
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherRepository weatherRepository;
        public WeatherForecastService(IWeatherRepository repository) 
        {
            this.weatherRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public IEnumerable<WeatherForecast> Get()
        {
            return this.weatherRepository.Get();
        }
    }
}
