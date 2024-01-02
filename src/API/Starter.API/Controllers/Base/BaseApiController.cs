using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Starter.API.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
