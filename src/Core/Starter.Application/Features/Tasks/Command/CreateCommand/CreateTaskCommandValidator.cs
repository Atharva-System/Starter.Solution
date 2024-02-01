using Starter.Application.UnitOfWork;

namespace Starter.Application.Features.Tasks.Command.CreateCommand;
public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommandRequest>
{
    private readonly IQueryUnitOfWork _query;
    public CreateTaskCommandValidator(IQueryUnitOfWork query)
    {
        _query = query;

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
