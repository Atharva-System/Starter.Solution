using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Starter.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //Request
        _logger.LogInformation($"Handling {typeof(TRequest).Name}");
        Type myType = request.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            object propValue = prop.GetValue(request, null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            _logger.LogInformation("{Property} : {@Value}", prop.Name, propValue);
        }

        var response = await next();

        //Response
        _logger.LogInformation($"Handled {typeof(TResponse).Name}");
        return response;
    }
}
