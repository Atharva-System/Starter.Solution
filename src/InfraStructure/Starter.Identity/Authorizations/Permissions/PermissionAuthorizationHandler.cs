using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Starter.Identity.Constants;
using Starter.Identity.Models;


namespace Starter.Identity.Authorizations.Permissions;
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    UserManager<ApplicationUser> _userManager;
    RoleManager<ApplicationRole> _roleManager;

    public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userEmail = context.User.FindFirstValue(ClaimTypes.Email);
        if (userEmail == null)
        {
            return;
        }

        // Get all the roles the user belongs to and check if any of the roles has the permission required
        // for the authorization to succeed.
        //var user1 = await _userManager.GetUserAsync(context.User);
        var user = await _userManager.FindByEmailAsync(userEmail);
        var userRoleNames = await _userManager.GetRolesAsync(user);

        // Materialize the user roles collection to avoid open data reader issues
        var userRoles = _roleManager.Roles
            .Where(x => userRoleNames.Contains(x.Name))
            .ToList();


        foreach (var role in userRoles)
        {
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var permissions = roleClaims
                                .Where(x => x.Type == IdentityRoleClaims.Permission &&
                                            x.Value == requirement.Permission &&
                                            x.Issuer == "LOCAL AUTHORITY")
                                .Select(x => x.Value);

            if (permissions.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
