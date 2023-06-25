﻿namespace Nexus.Common.Abstractions.Common;

/// <summary>
/// Represents an entity with an integer identifier.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    int Id { get; set; }
}
