using Microsoft.AspNetCore.Mvc;
using Starter.Application.Contracts.Identity;
using Starter.Application.Models.Authentication;

namespace Starter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;


        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {

            _logger = logger;
            _authService = authService;
        }


        [HttpPost("signin")]
        public async Task<ActionResult<AuthenticationResponse>> SignInAsync(AuthenticationRequest request)
        {
            return Ok(await _authService.AuthenticateAsync(request));
        }


        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        {
            return Ok(await _authService.RegisterAsync(request));
        }
    }
}
