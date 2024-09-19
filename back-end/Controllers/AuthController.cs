using back_end.Interfaces;
using back_end.Models;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResult = await _authService.Login(model.email, model.password);

            // restituisco un azione "OK" con il Token e i dettagli dell'utente
            return Ok(new
            {
                Token = authResult.Token,
                User = authResult.User
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.Register(model);

            return Ok(user);
        }

        /*
        [HttpGet("prova")]
        [Authorize(Roles = "Admin")]
        public IActionResult prova()
        {
            return Ok(new { message = "This is protected data." });
        }*/
    }
}

