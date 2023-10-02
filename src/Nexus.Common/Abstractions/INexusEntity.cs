namespace Nexus.Common.Abstractions;

/// <summary>
/// Represents an entity with an integer identifier.
/// </summary>
public interface INexusEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    int Id { get; set; }
}
