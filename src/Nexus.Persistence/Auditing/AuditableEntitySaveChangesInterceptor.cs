using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Nexus.Common;

namespace Nexus.Persistence.Auditing;

/// <summary>
/// Interceptor for auditing entity changes during DbContext.SaveChanges operation.
/// </summary>
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableEntitySaveChangesInterceptor"/> class with the specified dependencies.
    /// </summary>
    /// <param name="currentUserService">The service for retrieving information about the current user.</param>
    /// <param name="dateTime">The service for retrieving the current date and time.</param>
    public AuditableEntitySaveChangesInterceptor(
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    /// <summary>
    /// Intercept the SavingChanges event of the DbContext and update the auditable entities with auditing information.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="result">The result of the intercepted operation.</param>
    /// <returns>The modified result of the intercepted operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Intercept the SavingChangesAsync event of the DbContext and update the auditable entities with auditing information.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="result">The result of the intercepted operation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the modified result of the intercepted operation.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        foreach (EntityEntry<AuditableEntityBase> entry in context.ChangeTracker.Entries<AuditableEntityBase>())
        {
            if (entry.State == EntityState.Added)
            {
                // Set created by and created on properties
                entry.Entity.CreatedBy = _currentUserService.UserId ?? string.Empty;
                entry.Entity.CreatedOn = _dateTime.UtcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified ||
                HasChangedOwnedEntities(entry))
            {
                // Set modified by and modified on properties
                entry.Entity.ModifiedBy = _currentUserService.UserId ?? string.Empty;
                entry.Entity.ModifiedOn = _dateTime.UtcNow;
            }
        }
    }

    private static bool HasChangedOwnedEntities(EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
