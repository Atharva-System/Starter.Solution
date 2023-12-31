﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Starter.Blazor;
using Starter.Blazor.Core.Auth;
using Starter.Blazor.Modules.Login.Services;
using Blazored.LocalStorage;
using Starter.Blazor.Modules.ForgotPassword.Services;
using Starter.Blazor.Modules.ResetPassword.Services;
using Starter.Blazor.Modules.User.Services;
using Blazored.LocalStorage;
using Starter.Blazor.Core.AuthProviders;
using Starter.Blazor.Modules.ChangePassword.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration.GetValue<string>("AppConfig:ApiUrl");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(!string.IsNullOrEmpty(apiUrl) ? apiUrl : builder.HostEnvironment.BaseAddress),
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<ForgotPasswordService>();
builder.Services.AddScoped<ResetPasswordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChangePasswordServices, ChangePasswordServices>();
builder.Services.AddScoped<UserAuthID>();

await builder.Build().RunAsync();
