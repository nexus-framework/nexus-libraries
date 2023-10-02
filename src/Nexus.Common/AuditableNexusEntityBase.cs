using Nexus.Common.Abstractions;

namespace Nexus.Common;

/// <summary>
/// Represents an abstract base class for an auditable entity with string-based audit information.
/// </summary>
public abstract class AuditableNexusEntityBase : NexusEntityBase, IAuditable<string>
{
    /// <summary>
    /// Gets or sets the name of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who last modified the entity.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the entity was last modified.
    /// </summary>
    public DateTime ModifiedOn { get; set; }
}
