namespace Nexus.Persistence.Auditing;

/// <summary>
/// Represents a service for retrieving date and time information.
/// </summary>
public interface IDateTime
{
    /// <summary>
    /// Gets the current local date and time.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTime UtcNow { get; }
}
