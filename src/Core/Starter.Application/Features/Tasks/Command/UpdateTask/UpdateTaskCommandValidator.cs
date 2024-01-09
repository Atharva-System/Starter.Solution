using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Command.UpdateTask;
public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskRequestCommand>
{
    private readonly IQueryUnitOfWork _query;
    public UpdateTaskCommandValidator(IQueryUnitOfWork query)
    {
        _query = query;

        RuleFor(p => p.TaskName)
             .NotEmpty()
             .MustAsync(async (task, name, ct) => await _query.QueryRepository<Domain.Entities.Tasks>().GetAsync(c => c.TaskName!.ToLower() == name!.ToLower() && c.Id != task.Id) is null)
             .WithMessage((_, name) => $"Task {name} already exists!");
    }
}
