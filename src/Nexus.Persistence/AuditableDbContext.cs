using Microsoft.EntityFrameworkCore;
using Nexus.Persistence.Auditing;

namespace Nexus.Persistence;

/// <summary>
/// Represents a custom DbContext that includes auditing functionality for entities.
/// </summary>
public class AuditableDbContext : DbContextBase
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableDbContext"/> class with the specified options and auditing interceptor.
    /// </summary>
    /// <param name="options">The options for configuring the context.</param>
    /// <param name="auditableEntitySaveChangesInterceptor">The interceptor for auditing entity changes.</param>
    public AuditableDbContext(DbContextOptions options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    /// <summary>
    /// Configures additional options for the context during the configuration phase.
    /// </summary>
    /// <param name="optionsBuilder">The builder used to configure the context options.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}
