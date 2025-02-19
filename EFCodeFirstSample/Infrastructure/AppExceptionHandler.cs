using EFCodeFirstSample.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace EFCodeFirstSample.Infrastructure
{
    /// <summary>
    /// Application exception handler to handle all unhandled exception
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Diagnostics.IExceptionHandler" />
    public class AppExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger;

        public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Tries to handle the specified exception asynchronously within the ASP.NET Core pipeline.
        /// Implementations of this method can provide custom exception-handling logic for different scenarios.
        /// </summary>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse
            {
                ErrorMessage = exception.InnerException?.Message ?? exception.Message,
                CorrelationId = (string)(httpContext.Items[LogContextMiddleware.CorrelationIdIdentifier] ?? string.Empty)
            };

            //Log an error message and push Message as a property for Serilog, if use @ then json data will be shown as structured
            _logger.LogError(exception, "Exception occurred : {Message}", response.ErrorMessage);
            
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
