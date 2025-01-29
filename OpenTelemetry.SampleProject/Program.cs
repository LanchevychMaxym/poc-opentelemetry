
using OpenTelemetry.DataAccess;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Service;
using OpenTelemetry.Shared;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using Serilog;

namespace OpenTelemetry.SampleProject
{
    public class Program
    {
        const string serviceName = "OpenTelemetry.SampleProject.API";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
            builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

            
            builder.Logging.AddOpenTelemetry(options =>
            {
                options
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName)
                            .AddService(WeatherRepository.ActivitySourceName)
                            .AddService(WeatherForecastService.ActivitySourceName))
                    .AddConsoleExporter()
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri("http://localhost:4317");

                        otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                    });
            });
            builder.Services.AddOpenTelemetry()
                  .ConfigureResource(resource => resource
                    .AddService(serviceName)
                    .AddService(WeatherRepository.ActivitySourceName)
                    .AddService(WeatherForecastService.ActivitySourceName))
                  .WithTracing(tracing => tracing
                      .AddAspNetCoreInstrumentation()
                      .AddConsoleExporter()
                      .AddOtlpExporter(otlpOptions =>
                      {
                          otlpOptions.Endpoint = new Uri("http://localhost:4317");

                          otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                      }))
                  .WithMetrics(metrics => metrics
                      .AddAspNetCoreInstrumentation()
                      .AddConsoleExporter()
                      .AddOtlpExporter(otlpOptions =>
                      {
                          otlpOptions.Endpoint = new Uri("http://localhost:4317");

                          otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                      }));

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
