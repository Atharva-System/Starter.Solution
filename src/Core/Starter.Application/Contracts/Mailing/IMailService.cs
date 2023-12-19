using Starter.Application.Interfaces;

namespace Starter.Application.Contracts.Mailing;
public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}
