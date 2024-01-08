using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using Starter.Application.Exceptions;
using Starter.Application.Features.Common;

namespace Starter.API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ConvertException(context, ex);
        }
    }

    private Task ConvertException(HttpContext context, Exception exception)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        var result = string.Empty;

        switch (exception)
        {
            case FluentValidation.ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = (int)httpStatusCode,
                    Message = validationException?.Errors?.Select(error => error.ErrorMessage).FirstOrDefault()
                }); ;
                break;
            case BadRequestException badRequestException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = (int)httpStatusCode,
                    Messages = new List<string> { badRequestException.Message }
                    // You can include additional metadata if needed
                });
                break;
            case NotFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = (int)httpStatusCode,
                    Messages = new List<string> { "Resource not found" }
                    // You can include additional metadata if needed
                });
                break;
            case AuthenticationException:
            case UnauthorizedAccessException:
                httpStatusCode = HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = (int)httpStatusCode,
                    Messages = new List<string> { "Unauthorized access" }
                    // You can include additional metadata if needed
                });
                break;
            case ForbiddenAccessException:
                httpStatusCode = HttpStatusCode.Forbidden;
                result = JsonSerializer.Serialize(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = (int)httpStatusCode,
                    Messages = new List<string> { "Forbidden access" }
                    // You can include additional metadata if needed
                });
                break;
            case Exception:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = (int)httpStatusCode,
                    Messages = new List<string> { exception.Message }
                    // You can include additional metadata if needed
                });
                break;
        }

        context.Response.StatusCode = (int)httpStatusCode;

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }

        return context.Response.WriteAsync(result);
    }
}
