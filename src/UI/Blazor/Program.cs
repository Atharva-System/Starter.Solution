using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Starter.Blazor;
using Starter.Blazor.Core.Auth;
using Starter.Blazor.Modules.Login.Services;
using Starter.Blazor.Modules.ForgotPassword.Services;
using Starter.Blazor.Modules.ResetPassword.Services;
using Starter.Blazor.Modules.User.Services;
using Starter.Blazor.Core.AuthProviders;
using Starter.Blazor.Modules.ChangePassword.Services;
using Starter.Blazor.Modules.Projects.Services;
using Starter.Blazor.Core;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();

var apiUrl = builder.Configuration.GetValue<string>("AppConfig:ApiUrl");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(!string.IsNullOrEmpty(apiUrl) ? apiUrl : builder.HostEnvironment.BaseAddress),
}.EnableIntercept(sp));

builder.Services.AddHttpClientInterceptor();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ForgotPasswordService>();
builder.Services.AddScoped<ResetPasswordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IChangePasswordServices, ChangePasswordServices>();
builder.Services.AddScoped<UserAuthID>();
builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


await builder.Build().RunAsync();
