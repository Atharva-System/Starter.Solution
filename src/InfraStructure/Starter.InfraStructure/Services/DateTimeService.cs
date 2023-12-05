using Starter.Application.Contracts.Application;

namespace Starter.InfraStructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeOffset Now => DateTime.Now;
}
