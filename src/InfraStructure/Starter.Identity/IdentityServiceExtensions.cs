using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Starter.Application.Contracts.Identity;
using Starter.Application.Models.Authentication;
using Starter.Identity.Authorization;
using Starter.Identity.Database;
using Starter.Identity.Models;
using Starter.Identity.Services;

namespace Starter.Identity;

public static class IdentityServiceExtensions
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName)));


        services.AddScoped<AppIdentityDbContextInitialiser>();

        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();



        services.AddTransient<IAuthService, AuthService>();




        // Register Authorization Handlers
        services.AddSingleton<IAuthorizationHandler, ResourceOperationAuthorizationHandler>();

        // Register Authorization Policy Provider
        services.AddSingleton<IAuthorizationPolicyProvider, ResourceOperationAuthorizationPolicyProvider>();

        services.AddScoped<IAuthorizationHandler, ResourceOperationAuthorizationHandler>();





        //services.AddAuthorization(options =>
        //   options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
#pragma warning disable CS8604 // Possible null reference argument.
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    };
#pragma warning restore CS8604 // Possible null reference argument.

                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize("401 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize("403 Not authorized");
                            return context.Response.WriteAsync(result);
                        }
                    };
                });
    }
}
