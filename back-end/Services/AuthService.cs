using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace back_end.Services
{
	public class AuthService : IAuthService
	{
        private readonly JwtSettings _jwtSettings; // contiene le impostazione JWT
        private readonly ApplicationDbContext _dbContext;

		public AuthService(IOptions<JwtSettings> jwtSettings,
            ApplicationDbContext dbContext)
		{
            _jwtSettings = jwtSettings.Value;
            _dbContext = dbContext;
		}

        public async Task<(string token, User user)> Login(string email, string password)
        {
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Email == email);

            if(user != null && email == user.Email && password == user.PasswordHash)
            {
                // creazione e gestione dei token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                // chiave segreta per firmare il token convertita in un array di byte
                var Key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
                // descrive le caratteristiche del token JWT
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // identità dell'utente
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
                    }),
                    // data di scadenza del token
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    // credenziali per firmare il token
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Key),
                        SecurityAlgorithms.HmacSha256Signature),
                    // emittente del token
                    Issuer = _jwtSettings.Issuer,
                    // destinatario del token
                    Audience = _jwtSettings.Audience
                };

                // crea il token jwt sulla base del token descriptor
                var token = tokenHandler.CreateToken(tokenDescriptor);
                // converte il token in una stringa JWT
                var jwtToken = tokenHandler.WriteToken(token);

                // restituisco il bearer token e l'utente connesso
                return (jwtToken, user);
            }

            // se credenziali errate restituisco null
            return (null, null);
        }

        public Task<User> Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}

