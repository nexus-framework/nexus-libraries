namespace Nexus.Persistence.Abstractions;

/// <summary>
/// Represents a unit of work interface for managing transactions and data persistence.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Commits the changes made within the unit of work to the underlying data store.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back the changes made within the unit of work, discarding any pending changes.
    /// </summary>
    void Rollback();

    /// <summary>
    /// Begins a new transaction within the unit of work.
    /// </summary>
    void BeginTransaction();
}
