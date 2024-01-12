using System.ComponentModel;

namespace Starter.Blazor.Core.Services.IServices;

public interface INotificationService
{
    Task Success(string Message);
    Task Failure(List<string> Messages);
    Task Message(string Message);
}

public enum NotificationType
{
    [Description("normal")]
    Normal,
    [Description("info")]
    Info,
    [Description("success")]
    Success,
    [Description("warning")]
    Warning,
    [Description("error")]
    Error
}
