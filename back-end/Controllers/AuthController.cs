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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new {message = "Dati non validi"});
            }

            var authResult = await _authService.Login(model.email, model.password);

            // controllo se il login è avvenuto con successo
            // altrimenti restituisco una non autorizzazione con il messaggio
            // di errore
            if(!authResult.IsSuccess)
            {
                _logger.LogWarning($"Login fallito per {model.email}: {authResult.ErrorMessage}");
                return Unauthorized(new {message = authResult.ErrorMessage});
            }

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
                return BadRequest(new { message = "Dati non validi", errors = ModelState });
            }

            var (user, errorMessage) = await _authService.Register(model);

            if(user == null)
            {
                _logger.LogWarning($"Registrazione fallita per {model.Email}: {errorMessage}");
                return BadRequest(new { message = errorMessage });
            }
            return Ok(new {message = "Registrazione avvenuta con successo"});
        }
        
        [HttpGet("prova")]
        [Authorize(Roles = "Admin")]
        public IActionResult prova()
        {
            return Ok(new { message = "This is protected data." });
        }
    }
}

