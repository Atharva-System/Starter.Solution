using Starter.Application.Interfaces;

namespace Starter.Application.Contracts.Mailing;
public interface IEmailTemplateService : ITransientService
{
    string GenerateDefaultEmailTemplate<T>(T mailTemplateModel);
}
