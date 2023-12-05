using Starter.Application.Security;
using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;
using Starter.Domain.Enums;
using Starter.Domain.Events;

namespace Starter.Application.Features.Todos.Create;
//[Authorize(Roles = $"{Modules.TODO},{Action.Create}")]

[Authorize(ModuleType = nameof(ModuleTypes.TodoItem), Actions = "Create")]

public record CreateTodoItemCommand : IRequest<int>
{


    public string? Title { get; init; }

    public int Priority { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{


    private readonly ICommandUnitOfWork _commandUnitofWork;
    // private readonly IMapper _mapper;



    public CreateTodoItemCommandHandler(ICommandUnitOfWork command)//, IMapper mapper)
    {
        _commandUnitofWork = command;
        //_mapper = mapper;
    }

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

        //entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        //await _context.SaveChangesAsync(cancellationToken);

        //return entity.Id;
    }
}

