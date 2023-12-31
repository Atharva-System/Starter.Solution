﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Starter.Domain.Entities;

namespace Starter.Persistence.Database;
public static class AppDBInitialiserExtensions
{
    public static async Task InitialiseAppDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class AppDbContextInitialiser(ILogger<AppDbContext> logger, AppDbContext context)
{
    private readonly ILogger<AppDbContext> _logger = logger;

    private readonly AppDbContext _context = context;

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
        // Default data
        // Seed, if necessary
        if (!_context.TodoItems.Any())
        {
            _context.TodoItems.Add(new TodoItem { Title = "Make a todo list 📃", Done = false });

            await _context.SaveChangesAsync();
        }

    }
}
