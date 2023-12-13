using Starter.Application.Features.Common;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;
using Starter.Domain.Enums;
using Starter.Domain.Events;

namespace Starter.Application.Features.Todos.Create;

public sealed record CreateTodoItemCommandReqeust : IRequest<ApiResponse<int>>
{
    public string? Title { get; init; }

    public int Priority { get; init; }
}

public class CreateTodoItemCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateTodoItemCommandReqeust, ApiResponse<int>>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<ApiResponse<int>> Handle(CreateTodoItemCommandReqeust request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            Title = request.Title,
            Done = false,
            Priority = (PriorityLevel)request.Priority
        };

        //var validator = new CreateTodoItemCommandValidator(_commandUnitofWork);

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

