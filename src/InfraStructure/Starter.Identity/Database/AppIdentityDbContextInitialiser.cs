using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Starter.Identity.Authorization;
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

public class AppIdentityDbContextInitialiser(ILogger<AppIdentityDbContextInitialiser> logger, AppIdentityDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
{
    private readonly ILogger<AppIdentityDbContextInitialiser> _logger = logger;
    private readonly AppIdentityDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

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
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {


        //seed user, roles and role claims
        await new RoleClaimSeeder(_roleManager, _userManager, configuration).SeedRoleClaimsAsync();




        // Default roles
        //var administratorRole = new IdentityRole(Roles.Administrator);

        //if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        //{
        //    await _roleManager.CreateAsync(administratorRole);

        //    var adminRole = await _roleManager.FindByNameAsync(administratorRole.Name);

        //    await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.Todo.Create));

        //}

        // Default users
        //        var administrator = new ApplicationUser { UserName = configuration.GetSection("AppSettings")["UserEmail"], Email = configuration.GetSection("AppSettings")["UserEmail"] };

        //        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        //        {
        //#pragma warning disable CS8604 // Possible null reference argument.
        //            _ = await _userManager.CreateAsync(administrator, configuration.GetSection("AppSettings")["UserPassword"]);
        //#pragma warning restore CS8604 // Possible null reference argument.
        //            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
        //            {
        //                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        //            }
        //        }
    }
}
