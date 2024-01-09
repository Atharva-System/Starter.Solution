using Starter.Application.UnitOfWork;
using Starter.Domain.Entities;

namespace Starter.Application.Features.Projects.Command.UpdateProject;
public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IQueryUnitOfWork _query;
    public UpdateProjectCommandValidator(IQueryUnitOfWork query)
    {
        _query = query;

        RuleFor(p => p.ProjectName)
            .NotEmpty()
            .MustAsync(async (project, name, ct) => await _query.QueryRepository<Project>().GetAsync(c => c.ProjectName!.ToLower() == name!.ToLower() && c.Id != project.Id) is null)
            .WithMessage((_, name) => $"Project {name} already exists!");
    }
}
