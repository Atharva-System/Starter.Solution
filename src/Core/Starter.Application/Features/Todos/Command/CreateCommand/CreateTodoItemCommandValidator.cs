using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Todos.Command.CreateCommand;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommandReqeust>
{
    private readonly IQueryUnitOfWork _queryUnitOfWork;
    public CreateTodoItemCommandValidator(IQueryUnitOfWork query)
    {
        _queryUnitOfWork = query;
        RuleFor(d => d.Title).NotEmpty().NotEmpty().NotNull().WithMessage("{ProperyName} must not be empty!")
                .Length(2, 15).WithMessage("{ProperyName} must be between 2 and 15 characters!");

        RuleFor(d => d.Title)
             .MustAsync(async (category, title, ct) => await _queryUnitOfWork!.QueryRepository<TodoItem>().GetAsync(c => c.Title!.ToLower() == title!.ToLower()) is null)
             .WithMessage((_, title) => $"Title {title} already exists!");
    }
}
