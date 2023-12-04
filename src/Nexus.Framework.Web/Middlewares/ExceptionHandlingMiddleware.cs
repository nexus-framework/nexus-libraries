using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Nexus.Framework.Web.Middlewares;

/// <summary>
/// A middleware for handling exceptions.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the ExceptionHandlingMiddleware class.
    /// </summary>
    /// <param name="next">The next RequestDelegate in the pipeline.</param>
    /// <param name="logger">The logger to use for logging exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HttpContext for the request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "An internal error occurred");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An internal error occurred.");
        }
    }
}

/// <summary>
/// Extension methods for using the ExceptionHandlingMiddleware.
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>
    /// Adds the ExceptionHandlingMiddleware to the specified IApplicationBuilder's pipeline.
    /// </summary>
    /// <param name="builder">The IApplicationBuilder to add the middleware to.</param>
    /// <returns>The original app with the middleware configured.</returns>
    public static IApplicationBuilder UseNexusExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}