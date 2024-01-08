using Microsoft.EntityFrameworkCore;
using Starter.Identity.Constants;
using Starter.Application.Contracts.Caching;

namespace Starter.Identity.Services;
public partial class UsersService
{
    public async Task<List<string>> GetPermissionAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        _ = user ?? throw new Exception("Authentication Failed.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var roles in await _roleManager.Roles.
            Where(r => userRoles.Contains(r.Name!)).ToListAsync(cancellationToken))
        {
            permissions.AddRange(await _db.RoleClaims
                .Where(rc => rc.RoleId == roles.Id && rc.ClaimType == IdentityRoleClaims.Permission)
                .Select(rc => rc.ClaimValue!)
                .ToListAsync(cancellationToken));
        }

        return permissions.Distinct().ToList();
    }

    public async Task<bool> HasPermissionAsync(string? userId, string permission, CancellationToken cancellationToken)
    {
        var permissions = await _cache.GetOrSetAsync(
            _cacheKey.GetCacheKey(IdentityRoleClaims.Permission, userId),
            () => GetPermissionAsync(userId, cancellationToken),
            TimeSpan.FromDays(1),
            cancellationToken: cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }
}
