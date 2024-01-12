using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Starter.Blazor;
using Starter.Blazor.Core.Auth;
using Starter.Blazor.Core.AuthProviders;
using Starter.Blazor.Modules.ChangePassword.Services;
using Starter.Blazor.Modules.ForgotPassword.Services;
using Starter.Blazor.Modules.Login.Services;
using Starter.Blazor.Modules.ForgotPassword.Services;
using Starter.Blazor.Modules.ResetPassword.Services;
using Starter.Blazor.Modules.Task.Services;
using Starter.Blazor.Modules.User.Services;
using Starter.Blazor.Core.AuthProviders;
using Starter.Blazor.Modules.ChangePassword.Services;
using Starter.Blazor.Modules.Projects.Services;
using Starter.Blazor.Core;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Core.Services;
using Starter.Blazor.Modules.Login.Services.IServices;
using Starter.Blazor.Core.Authentication;

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


//Utility services registered
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IHttpInterceptorService, HttpInterceptorService>();
builder.Services.AddScoped<IApiHandler,ApiHandler>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AppStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AppStateProvider>();
builder.Services.AddTransient<AuthenticationHeaderHandler>();
builder.Services.AddBlazoredModal();
builder.Services.AddScoped<ITaskService, TaskServices>();
builder.Services.AddScoped<IModalService, ModalService>();


builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<ForgotPasswordService>();
builder.Services.AddScoped<ResetPasswordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IChangePasswordServices, ChangePasswordServices>();
builder.Services.AddScoped<UserAuthID>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<UserService>();

await builder.Build().RunAsync();
