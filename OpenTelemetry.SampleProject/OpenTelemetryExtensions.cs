using OpenTelemetry.Trace;

namespace OpenTelemetry.SampleProject
{
    public static class OpenTelemetryExtensions
    {
        public static TracerProviderBuilder UseSources(this TracerProviderBuilder tracer, params string[] sources)
        {
            foreach (var source in sources)
            {
                tracer.AddSource(source);
            }
            return tracer;
        }
    }
}
