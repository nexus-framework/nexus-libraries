using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nexus.Common.Attributes;
using Nexus.Persistence.Auditing;

namespace Nexus.Framework.Web.Services;

/// <summary>
/// Represents a service for retrieving the current user's information.
/// </summary>
[NexusService<ICurrentUserService>(NexusServiceLifeTime.Singleton)]
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The accessor for accessing the HTTP context.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the ID of the current user.
    /// </summary>
    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
