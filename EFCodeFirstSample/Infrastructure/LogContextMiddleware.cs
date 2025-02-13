using Serilog.Context;

namespace EFCodeFirstSample.Infrastructure
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public const string CorrelationIdIdentifier = "CorrelationId";

        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            using (LogContext.PushProperty(CorrelationIdIdentifier, GetCorrelationId(context)))
            {
                return _next(context);
            }
        }

        private string GetCorrelationId(HttpContext context)
        {
            return (string)(context.Items[CorrelationIdIdentifier] ??= Guid.NewGuid().ToString());
        }
    }
}
