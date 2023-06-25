using Microsoft.EntityFrameworkCore;

namespace Nexus.Persistence;

/// <summary>
/// Base class for custom DbContext implementations.
/// </summary>
public class DbContextBase : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbContextBase"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options for configuring the DbContext.</param>
    protected DbContextBase(DbContextOptions options)
        : base(options)
    {
    }
}
