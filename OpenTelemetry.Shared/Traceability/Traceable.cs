using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace OpenTelemetry.Shared.Traceability
{
    public class Traceable<TType>
        where TType : class
    {
        public static string ActivitySourceName = typeof(TType).Name;
        public static ActivitySource ActivitySource { get; } = new ActivitySource(ActivitySourceName);

        public TResult TraceAction<TResult>(Func<TResult> action, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "")
        {
            string callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            using (var activity = ActivitySource.StartActivity($"{callerClassName}.{callerName}"))
            {
                TResult result;

                try
                {
                    result = action();
                    activity?.SetTag($"{callerName}.Result", result?.ToString());
                }
                catch (Exception ex)
                {
                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                    throw;
                }
                finally
                {
                    activity?.Stop();
                }

                return result;
            }
        }
        public void TraceAction(Action action, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "")
        {
            string callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            using (var activity = ActivitySource.StartActivity($"{callerClassName}.{callerName}"))
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                    throw;
                }
                finally
                {
                    activity?.Stop();
                }
            }
        }
    }
}
