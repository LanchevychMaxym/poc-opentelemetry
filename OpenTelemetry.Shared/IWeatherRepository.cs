namespace OpenTelemetry.Shared
{
    public interface IWeatherRepository
    {
        public IEnumerable<WeatherForecast> Get();
    }
}
