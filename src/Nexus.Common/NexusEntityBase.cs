using Nexus.Common.Abstractions;

namespace Nexus.Common;

/// <summary>
/// Represents an abstract base class for an entity with an integer identifier.
/// </summary>
public abstract class NexusEntityBase : INexusEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public int Id { get; set; }
}
