using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;
using Starter.Domain.Events;

namespace Starter.Application.Features.Todos.Create;

public sealed record CreateTodoItemCommand : IRequest<ApiResponse<int>>
{
    public string? Title { get; init; }

    public int Priority { get; init; }
}

public class CreateTodoItemCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateTodoItemCommand, ApiResponse<int>>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<ApiResponse<int>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        await _commandUnitofWork.CommandRepository<TodoItem>().AddAsync(entity);
        var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
        var response = new ApiResponse<int>
        {
            Success = saveResult > 0,
            StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
            Data = saveResult,
            Message = saveResult > 0 ? $"Todo {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} todo item."

        };
        return response;
    }
}

