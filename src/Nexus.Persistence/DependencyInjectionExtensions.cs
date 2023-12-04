using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nexus.Persistence;
using Nexus.Persistence.Auditing;
using Steeltoe.Connector.PostgreSql;
using Steeltoe.Connector.PostgreSql.EFCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// A static class that provides extension methods for adding core persistence to the service collection.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds core persistence-related services to the service collection.
    /// </summary>
    /// <typeparam name="TContext">The type of the DbContext.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the configuration settings.</param>
    /// <remarks>
    /// This method adds the DbContext of type <typeparamref name="TContext"/> using Npgsql as the database provider.
    /// It also registers the <see cref="AuditableEntitySaveChangesInterceptor"/> as a scoped service, which is used to
    /// intercept and modify the changes made to auditable entities before saving them to the database.
    /// Additionally, it adds the PostgreSQL health contributor, which enables health checks for the PostgreSQL database
    /// using the configuration provided.
    /// </remarks>
    public static void AddNexusPersistence<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => { options.UseNpgsql(configuration); });
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddPostgresHealthContributor(configuration);
    }

    /// <summary>
    /// Adds core persistence-related services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the configuration settings.</param>
    /// <param name="assembly">The assembly containing the DbContexts to add.</param>
    public static void AddNexusPersistence(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        List<Type> dbContextTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AuditableDbContext))).ToList();
        if (dbContextTypes.Count == 0)
        {
            return;
        }
        
        Type extensionMethodClassType = typeof(EntityFrameworkServiceCollectionExtensions);
        MethodInfo[] extensionMethods = extensionMethodClassType.GetMethods(BindingFlags.Static | BindingFlags.Public);
        MethodInfo? addDbContextMethod = extensionMethods.FirstOrDefault(method => method.Name == "AddDbContext" && method.GetGenericArguments().Length == 1);
        if (addDbContextMethod == null)
        {
            throw new NexusPersistenceException(NexusPersistenceException.UnableToRegisterNexusPersistence);
        }
        
        foreach (Type dbContextType in dbContextTypes)
        {
            MethodInfo genericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);
            object[] parameters =
            {
                services,
                new Action<DbContextOptionsBuilder>(options => options.UseNpgsql(configuration)),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped
            };
            genericMethod.Invoke(null, parameters);
        }
        
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddPostgresHealthContributor(configuration);
    }
}