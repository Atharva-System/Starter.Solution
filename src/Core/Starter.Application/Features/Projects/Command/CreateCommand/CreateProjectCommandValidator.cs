using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Command.CreateCommand;
public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommandRequest>
{
    private readonly IQueryUnitOfWork _query;
    public CreateProjectCommandValidator(IQueryUnitOfWork query)
    {
        _query = query;

        RuleFor(p => p.ProjectName)
            .NotEmpty()
            .MustAsync(async (project, name, ct) => await _query.QueryRepository<Project>().GetAsync(c => c.ProjectName!.ToLower() == name!.ToLower()) is null)
            .WithMessage((_, name) => $"Project {name} already exists!");

        RuleFor(p => p.StartDate)
            .NotEmpty()
            .WithMessage((_, name) => "Start Date is required");

        RuleFor(p => p.EndDate)
            .NotEmpty()
            .WithMessage((_, name) => "End Date is required");

        RuleFor(p => p.EstimatedHours)
            .NotEmpty()
            .WithMessage((_, name) => "Estimated Hours is required");
    }
}
