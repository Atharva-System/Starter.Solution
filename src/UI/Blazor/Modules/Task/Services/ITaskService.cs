using Blazored.Modal;
using Blazored.Modal.Services;
using Starter.Blazor.Modules.Task.Model;

namespace Starter.Blazor.Modules.Task.Services;

public interface ITaskService : IModalService
{
    new Task<string> CloseAsync(ModalResult result = null);
    new Task<string> CancelAsync();

    new event Func<Task<string>> OnClose;
    Task<string> CreateTaskAsync(TaskDetailsDto dto);
}
