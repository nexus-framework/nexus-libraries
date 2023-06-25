using Microsoft.EntityFrameworkCore;
using Nexus.Common.Abstractions.Common;
using Nexus.Persistence.Abstractions;

namespace Nexus.Persistence;

/// <summary>
/// Implementation of the custom repository using Entity Framework for data access.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class EfCustomRepository<T> : ICustomRepository<T>
    where T : class, IEntity
{
    /// <summary>
    /// The <see cref="DbSet{TEntity}"/> representing the entities of type <typeparamref name="T"/>.
    /// </summary>
    protected readonly DbSet<T> DbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="EfCustomRepository{T}"/> class with the specified <see cref="DbContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> instance.</param>
    protected EfCustomRepository(DbContext context)
    {
        DbSet = context.Set<T>();
    }

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/> asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation, containing the list of entities.</returns>
    public Task<List<T>> AllAsync(CancellationToken cancellationToken = default)
    {
        return DbSet.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves an entity of type <typeparamref name="T"/> by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The entity with the specified ID, or <c>null</c> if not found.</returns>
    public T? GetById(int id)
    {
        return DbSet.Find(id);
    }

    /// <summary>
    /// Retrieves an entity of type <typeparamref name="T"/> by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation, containing the entity with the specified ID, or <c>null</c> if not found.</returns>
    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Adds a new entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    /// <summary>
    /// Deletes an existing entity of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }
}
