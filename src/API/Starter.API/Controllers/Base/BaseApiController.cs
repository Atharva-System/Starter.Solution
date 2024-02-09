using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.InfraStructure.Cors;

namespace Starter.API.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected string GetOriginFromRequest(IConfiguration _configuration)
    {
        string requestSource = Convert.ToString(HttpContext.Request.Headers["request-source"]);

        var corsSettings = _configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

        if (corsSettings != null)
        {
            if (corsSettings.Angular is not null && requestSource == "angular")
            {
                return corsSettings.Angular;
            }
            else if (corsSettings.Blazor is not null && requestSource == "blazor")
            {
                return corsSettings.Blazor;
            }
            else if (corsSettings.Vue is not null && requestSource == "vue")
            {
                return corsSettings.Vue;
            }
            else if (corsSettings.React is not null && requestSource == "react")
            {
                return corsSettings.React;
            }
        }
        return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }
}
