﻿using EFCodeFirstSample.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace EFCodeFirstSample.Handlers
{
    /// <summary>
    /// Application exception handler to handle all unhandled exception
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Diagnostics.IExceptionHandler" />
    public class AppExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// Tries to handle the specified exception asynchronously within the ASP.NET Core pipeline.
        /// Implementations of this method can provide custom exception-handling logic for different scenarios.
        /// </summary>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse
            {
                ErrorMessage = exception.Message,
                TrackingId = "" //
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
