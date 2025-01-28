using System.Diagnostics;

namespace OpenTelemetry.Shared.Traceability
{
    public class Traceable<TType>
        where TType : class
    {
        internal const string ActivitySourceName = nameof(TType);
        public static ActivitySource ActivitySource { get; } = new ActivitySource(ActivitySourceName);

        public TResult TraceAction<TResult>(Func<TResult> action)
        {
            using (var activity = ActivitySource.StartActivity(action.Method.Name))
            {
                TResult result;

                try
                {
                    result = action();
                    activity?.SetTag($"{action.Method.Name}.Result", result);
                }
                catch (Exception ex)
                {
                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                    throw;
                }

                return result;
            }
        }
        public void TraceAction(Action action)
        {
            using (var activity = ActivitySource.StartActivity(action.Method.Name))
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
            }
        }
    }
}
