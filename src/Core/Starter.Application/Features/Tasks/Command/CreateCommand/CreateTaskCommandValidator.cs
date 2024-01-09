using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Command.CreateCommand;
public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommandRequest>
{
    private readonly IQueryUnitOfWork _query;
    public CreateTaskCommandValidator(IQueryUnitOfWork query)
    {
        _query = query;

        RuleFor(p => p.TaskName)
            .NotEmpty()
            .MustAsync(async (task, name, ct) => await _query.QueryRepository<Domain.Entities.Tasks>().GetAsync(c => c.TaskName!.ToLower() == name!.ToLower()) is null)
            .WithMessage((_, name) => $"Task {name} already exists!");

        RuleFor(p => p.StartDate)
           .NotEmpty()
           .WithMessage((_, name) => "Start Date is required");

        RuleFor(p => p.EndDate)
            .NotEmpty()
            .WithMessage((_, name) => "End Date is required");

        RuleFor(p => p.Status)
            .NotEmpty()
            .WithMessage((_, name) => "Status is required");

        RuleFor(p => p.Priority)
            .NotEmpty()
            .WithMessage((_, name) => "Priority is required");
    }
}
