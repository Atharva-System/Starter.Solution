﻿using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;
using Starter.Domain.Events;

namespace Starter.Application.Features.Todos.Create;

public sealed record CreateTodoItemCommand : IRequest<int>
{
    public string? Title { get; init; }

    public int Priority { get; init; }
}

public class CreateTodoItemCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly ICommandUnitOfWork _commandUnitofWork = command;

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        await _commandUnitofWork.CommandRepository<TodoItem>().AddAsync(entity);

        return await _commandUnitofWork.SaveAsync(cancellationToken);

    }
}

