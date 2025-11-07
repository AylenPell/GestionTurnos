using Application.ExternalServices;
using Contracts.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] AuthCredentials credentials)
        {
            var token = _authService.Login(credentials, out string message);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(message);
            }
            return Ok(token);
        }

    }
}
