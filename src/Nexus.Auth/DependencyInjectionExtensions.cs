using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Nexus.Auth;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring dependency injection for authentication and authorization.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// The list of actions for CRUD operations.
    /// </summary>
    private static readonly string[] Actions = { "read", "write", "update", "delete" };

    /// <summary>
    /// Gets the CRUD policies for the specified resource using the default list of actions.
    /// </summary>
    /// <param name="resource">The resource for which to generate CRUD policies.</param>
    /// <returns>The list of CRUD policies.</returns>
    private static List<string> GetCrudPolicies(string resource)
    {
        return GetCrudPolicies(resource, Actions.ToList());
    }

    /// <summary>
    /// Gets the CRUD policies for the specified resource using the specified list of actions.
    /// </summary>
    /// <param name="resource">The resource for which to generate CRUD policies.</param>
    /// <param name="resourceActions">The list of actions for CRUD operations.</param>
    /// <returns>The list of CRUD policies.</returns>
    private static List<string> GetCrudPolicies(string resource, List<string> resourceActions)
    {
        return resourceActions.Select(action => $"{action}:{resource}").ToList();
    }

    /// <summary>
    /// Adds the CRUD policies to the authorization options.
    /// </summary>
    /// <param name="options">The authorization options.</param>
    /// <param name="policies">The list of CRUD policies to add.</param>
    private static void AddCrudPolicies(this AuthorizationOptions options, List<string> policies)
    {
        foreach (string policy in policies)
        {
            options.AddPolicy(policy, p => p.Requirements.Add(new ScopeRequirement(policy)));
        }
    }

    /// <summary>
    /// Adds the core authentication and authorization services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration object.</param>
    /// <param name="policies">The list of CRUD policies.</param>
    private static void AddCoreAuth(this IServiceCollection services, IConfiguration configuration, List<string> policies)
    {
        Auth0Settings auth0Settings = new ();
        configuration.GetRequiredSection(nameof(Auth0Settings)).Bind(auth0Settings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = auth0Settings.Authority;
            options.Audience = auth0Settings.Audience;
        });

        services.AddSingleton<IAuthorizationHandler, ScopeRequirementHandler>();
        services.AddAuthorization(options => { options.AddCrudPolicies(policies); });
    }

    /// <summary>
    /// Adds the core authentication and authorization services to the service collection for the specified resource.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration object.</param>
    /// <param name="resourceName">The name of the resource.</param>
    public static void AddCoreAuth(this IServiceCollection services, IConfiguration configuration, string resourceName)
    {
        List<string> policies = GetCrudPolicies(resourceName);
        services.AddCoreAuth(configuration, policies);
    }

    /// <summary>
    /// Adds the core authentication and authorization services to the service collection for the specified resource and actions.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration object.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <param name="resourceActions">The list of actions for the resource.</param>
    public static void AddCoreAuth(this IServiceCollection services, IConfiguration configuration, string resourceName, List<string> resourceActions)
    {
        List<string> policies = GetCrudPolicies(resourceName, resourceActions);
        services.AddCoreAuth(configuration, policies);
    }
}
