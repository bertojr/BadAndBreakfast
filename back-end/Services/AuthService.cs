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
        private readonly JwtSettings _jwtSettings;
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
                var tokenHandler = new JwtSecurityTokenHandler(); // per creare e validare i token JWT
                var Key = Encoding.ASCII.GetBytes(_jwtSettings.Key); // conversione della chiave in un array di byte
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Key),
                        SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettings.Issuer,
                    Audience = _jwtSettings.Audience
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return (jwtToken, user);
            }

            return (null, null);
        }

        public Task<User> Register(User user)
        {
            throw new NotImplementedException();
        }
    }
}

