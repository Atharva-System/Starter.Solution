using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Starter.Identity.Models;

namespace Starter.Identity.Authorization;
public class RoleClaimSeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    private readonly IConfiguration _configuration = configuration;



    public async Task SeedRoleClaimsAsync()
    {
        var rolesClaim = new RolesClaim();

        foreach (var roleClaim in rolesClaim.RoleClaims)
        {
            string roleName = roleClaim[0];
            string resource = roleClaim[1];
            string permission = roleClaim[2];

            await SeedRoleClaims(roleName, resource, permission);
        }
    }

    private async Task SeedRoleClaims(string roleName, string resourceName, string permission)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            role = await _roleManager.FindByNameAsync(roleName);
        }
        // Map resource to claim and add the claim
        var resourceClaim = RoleClaimMapper.MapResourceToClaim(resourceName, permission);

        // Check if the claim already exists for the role
        var existingClaim = await _roleManager.GetClaimsAsync(role!);
        if (!existingClaim.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == resourceClaim))
        {
            await _roleManager.AddClaimAsync(role!, new Claim(CustomClaimTypes.Permission, resourceClaim));
        }



        // Example: Assign the role to a user
        var user = new ApplicationUser { UserName = _configuration["AppSettings:UserEmail"], Email = _configuration["AppSettings:UserEmail"] };


        if (await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == _configuration["AppSettings:UserEmail"]!.ToString().ToUpper())
            is not ApplicationUser adminUser)
        {
            await _userManager.CreateAsync(user, _configuration["AppSettings:UserPassword"]!);
            await _userManager.AddToRoleAsync(user, roleName);
        }



    }

    //public async Task SeedRoleClaimsAsync()
    //{
    //    var roleFields = typeof(Roles).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

    //    foreach (var roleField in roleFields)
    //    {
    //        var roleName = roleField.GetValue(null) as string;

    //        if (!string.IsNullOrEmpty(roleName))
    //        {
    //            await SeedRoleClaims(roleName);
    //        }
    //    }
    //}

    private async Task SeedRoleClaims(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            role = await _roleManager.FindByNameAsync(roleName);
        }


        var administrator = new ApplicationUser { UserName = configuration.GetSection("AppSettings")["UserEmail"], Email = configuration.GetSection("AppSettings")["UserEmail"] };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
#pragma warning disable CS8604 // Possible null reference argument.
            _ = await _userManager.CreateAsync(administrator, configuration.GetSection("AppSettings")["UserPassword"]);
#pragma warning restore CS8604 // Possible null reference argument.
            if (!string.IsNullOrWhiteSpace(role.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { role.Name });
            }
        }



        // Add role claims for the specific role
        // Example: Add a claim for the role itself
        await _roleManager.AddClaimAsync(role, new Claim(Resources.TodoItem, Permissions.Create));

        // Add additional role claims as needed
    }
}

