using back_end.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var (token, user) = await _authService.Login(model.email, model.password);

            if(token == null)
            {
                return Unauthorized(new {message = "Credenziali invalide"});
            }

            return Ok(new
            {
                Token = token,
                User = user
            });
        }

        
        [HttpGet("prova")]
        [Authorize]
        public IActionResult prova()
        {
            return Ok(new { message = "This is protected data." });
        }
    }

    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}

