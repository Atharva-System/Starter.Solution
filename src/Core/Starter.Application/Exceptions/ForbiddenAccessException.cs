using System.Net;

namespace Starter.Application.Exceptions;

public class ForbiddenAccessException : CustomException
{
    public ForbiddenAccessException(string message, List<string>? errors = default) 
        : base(message, errors, HttpStatusCode.Forbidden) { }
}
