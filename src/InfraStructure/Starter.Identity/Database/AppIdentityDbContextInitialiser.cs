using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Starter.Domain.Constant;
using Starter.Identity.Authorizations;
using Starter.Identity.Constants;
using Starter.Identity.Models;

namespace Starter.Identity.Database;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<AppIdentityDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class AppIdentityDbContextInitialiser(ILogger<AppIdentityDbContextInitialiser> logger, AppIdentityDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
{
    private readonly ILogger<AppIdentityDbContextInitialiser> _logger = logger;
    private readonly AppIdentityDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    private readonly IConfiguration _configuration = configuration;

    public async Task InitialiseAsync()
    {
        var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {

            try
            {
                await _context.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
        var lastAppliedMigration = (await _context.Database.GetAppliedMigrationsAsync()).Last();

        Console.WriteLine($"You're on schema version: {lastAppliedMigration}");
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedSuperAdminUserAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }



    //--- Seed all the roles in the system

    private async Task SeedRolesAsync()
    {
        foreach (string roleName in Constants.IdentityRole.DefaultRoles)
        {
            var r = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                is not Models.ApplicationRole role)
            {
                // Create the role
                _logger.LogInformation("Seeding {role} Role for '{app}'.", roleName, _configuration["AppSettings:UserEmail"]);
                role = new Models.ApplicationRole(roleName, $"{roleName}-System Defined Role");
                await _roleManager.CreateAsync(role);
            }

            // Assign permissions
            if (roleName == Constants.IdentityRole.Administrator)
            {
                await AssignPermissionsToRoleAsync(AllPermissions.Root, role);
            }
            else if (roleName == Constants.IdentityRole.SubAdmin)
            {
                await AssignPermissionsToRoleAsync(AllPermissions.Admin, role);
            }
            else if (roleName == Constants.IdentityRole.User)
            {
                await AssignPermissionsToRoleAsync(AllPermissions.User, role);
            }

        }
    }


    private async Task AssignPermissionsToRoleAsync(IReadOnlyList<Permission> permissions, Models.ApplicationRole role)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!currentClaims.Any(c => c.Type == IdentityRoleClaims.Permission && c.Value == permission.Name))
            {
                _logger.LogInformation("Seeding {role} Permission '{permission}'.", role.Name, permission.Name);

                var claim = new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = IdentityRoleClaims.Permission,
                    ClaimValue = permission.Name,
                    CreatedBy = "IdentityDbSeeder"
                };

                // Add the claim to the role
                await _roleManager.AddClaimAsync(role, claim.ToClaim());

                //await _roleManager.AddClaimAsync(role, new Claim(IdentityRoleClaims.Permission, Permissions.Todo.Create));

                //dbContext.RoleClaims.Add(new ApplicationRoleClaim
                //{
                //    RoleId = role.Id,
                //    ClaimType = CPClaims.Permission,
                //    ClaimValue = permission.Name,
                //    CreatedBy = "IdentityDbSeeder"
                //});

                //await dbContext.SaveChangesAsync();
            }
        }
    }



    private async Task SeedSuperAdminUserAsync()
    {
        if (await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == _configuration["AppSettings:UserEmail"])
            is not ApplicationUser adminUser)
        {
            adminUser = new ApplicationUser()
            {
                FirstName = _configuration["AppSettings:Firstname"],
                LastName = Roles.Administrator,
                Email = _configuration["AppSettings:UserEmail"],
                UserName = _configuration["AppSettings:UserEmail"],
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
                IsSuperAdmin = true,
            };

            _logger.LogInformation("Seeding Default Admin User for '{id}'.", adminUser.Id);
            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, _configuration["AppSettings:UserPassword"]!.ToString());
            await _userManager.CreateAsync(adminUser);
        }

        // Assign role to user
        if (!await _userManager.IsInRoleAsync(adminUser, Constants.IdentityRole.Administrator))
        {
            _logger.LogInformation("Assigning Super Admin Role to Admin User for '{id}'.", adminUser.Id);
            await _userManager.AddToRoleAsync(adminUser, Constants.IdentityRole.Administrator);
        }
    }


    //public async Task TrySeedAsync()
    //{


    //    //seed user, roles and role claims
    //    //await new RoleClaimSeeder(_roleManager, _userManager, configuration).SeedRoleClaimsAsync();




    //    // Default roles
    //    //var administratorRole = new IdentityRole(Roles.Administrator);

    //    //if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
    //    //{
    //    //    await _roleManager.CreateAsync(administratorRole);

    //    //    var adminRole = await _roleManager.FindByNameAsync(administratorRole.Name);

    //    //    await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.Todo.Create));

    //    //}

    //    // Default users
    //    //        var administrator = new ApplicationUser { UserName = configuration.GetSection("AppSettings")["UserEmail"], Email = configuration.GetSection("AppSettings")["UserEmail"] };

    //    //        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
    //    //        {
    //    //#pragma warning disable CS8604 // Possible null reference argument.
    //    //            _ = await _userManager.CreateAsync(administrator, configuration.GetSection("AppSettings")["UserPassword"]);
    //    //#pragma warning restore CS8604 // Possible null reference argument.
    //    //            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
    //    //            {
    //    //                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
    //    //            }
    //    //        }
    //}
}
