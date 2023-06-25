using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Nexus.Framework.Web.Filters;

/// <summary>
/// Represents a filter for logging action execution.
/// </summary>
public class LoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingFilter"/> class.
    /// </summary>
    /// <param name="logger">The logger used for logging.</param>
    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes the filter asynchronously.
    /// </summary>
    /// <param name="context">The context of the action being executed.</param>
    /// <param name="next">The delegate representing the next action execution.</param>
    /// <returns>A task that represents the asynchronous action execution.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogDebug("Endpoint call start: {endpoint}", context.ActionDescriptor.DisplayName);

        Stopwatch sw = Stopwatch.StartNew();
        await next();
        sw.Stop();

        _logger.LogDebug("Endpoint call end: {endpoint} taking {duration}ms", context.ActionDescriptor.DisplayName,
            sw.ElapsedMilliseconds.ToString());
    }
}
