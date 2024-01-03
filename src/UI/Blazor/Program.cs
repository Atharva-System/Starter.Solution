﻿using Starter.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Starter.Blazor.Modules.User.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Starter.Blazor.Core.Auth;
using Starter.Blazor.Modules.Login.Services;
using Blazored.LocalStorage;
using Starter.Blazor.Core;
using Toolbelt.Blazor.Extensions.DependencyInjection;

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
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<HttpInterceptorService>();

await builder.Build().RunAsync();
