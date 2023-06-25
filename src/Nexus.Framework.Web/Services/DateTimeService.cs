using Nexus.Persistence.Auditing;

namespace Nexus.Framework.Web.Services;

/// <summary>
/// Represents a service for retrieving the current date and time.
/// </summary>
public class DateTimeService : IDateTime
{
    /// <summary>
    /// Gets the current local date and time.
    /// </summary>
    public DateTime Now => DateTime.Now;

    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;
}
