using Microsoft.JSInterop;
using Starter.Blazor.Core.Services.IServices;

namespace Starter.Blazor.Core.Services;

public class NotificationService : INotificationService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly string toastScriptConstant = @"
        const toast = window.Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 3000,
            showCloseButton: true,
        });";
    public NotificationService(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }

    public async Task Failure(List<string> Messages)
    {
        await this.ShowToaster(String.Join(",",Messages), NotificationType.Error.ToString().ToLower());
    }

    public async Task Message(string Message)
    {
        await this.ShowToaster(Message, NotificationType.Info.ToString().ToLower());
    }

    public async Task Success(string Message)
    {
        await this.ShowToaster(Message, NotificationType.Success.ToString().ToLower());
    }

    private async Task ShowToaster(string message,string type)
    {
        string toastScript = $"{toastScriptConstant} toast.fire({{ icon: `{type}`,title: '{message}' }});";
        _jsRuntime.InvokeVoidAsync("eval", toastScript);
    }
}
