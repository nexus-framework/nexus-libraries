using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nexus.Persistence.Abstractions;

namespace Nexus.Persistence;

/// <summary>
/// Base implementation of the unit of work pattern using Entity Framework.
/// </summary>
public class UnitOfWorkBase : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWorkBase"/> class with the specified <see cref="DbContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> instance.</param>
    protected UnitOfWorkBase(DbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }
    
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
    
    /// <summary>
    /// Commits the changes made in the unit of work to the underlying database.
    /// </summary>
    public void Commit()
    {
        try
        {
            _context.SaveChanges();
            _transaction?.Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
    }

    /// <summary>
    /// Rolls back the changes made in the unit of work.
    /// </summary>
    public void Rollback()
    {
        _transaction?.Rollback();
    }
}
