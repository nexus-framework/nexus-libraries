using Nexus.Common.Abstractions;

namespace Nexus.Persistence.Auditing;

/// <summary>
/// Represents a service for retrieving information about the current user.
/// </summary>
public interface ICurrentUserService : INexusService
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    string? UserId { get; }
}
