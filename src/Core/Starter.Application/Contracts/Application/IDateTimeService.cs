namespace Starter.Application.Contracts.Application;

public interface IDateTimeService
{
    DateTimeOffset Now { get; }
}
