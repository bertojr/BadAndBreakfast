using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class AuthService : IAuthService
	{
        private readonly JwtSettings _jwtSettings; // contiene le impostazione JWT
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AuthService> _logger;

		public AuthService(IOptions<JwtSettings> jwtSettings,
            ApplicationDbContext dbContext, ILogger<AuthService> logger)
		{
            _jwtSettings = jwtSettings.Value;
            _dbContext = dbContext;
            _logger = logger;
		}

        public async Task<AuthResult> Login(string email, string password)
        {
            try
            {
                var user = await _dbContext.Users
                    .AsNoTracking()
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Email non trovata"
                    };
                }

                if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Password errata"
                    };
                }

                // creazione e gestione dei token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                // chiave segreta per firmare il token convertita in un array di byte
                var Key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

                // creo le claim con i ruoli
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
                };

                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                };

                // descrive le caratteristiche del token JWT
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // identità dell'utente
                    Subject = new ClaimsIdentity(claims),
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
                return new AuthResult
                {
                    IsSuccess = true,
                    Token = jwtToken,
                    User = user
                };
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Errore nel database durante il login");
                return new AuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Errore interno del server"
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Errore interno del server");
                return new AuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Errore interno del server"
                };
            }
        }

        public async Task<(User user, string errorMessage)> Register(LoginModel model)
        {
            if(await _dbContext.Users.AnyAsync(u => u.Email == model.email))
            {
                return (null, "Email già in uso");
            }

            try
            {
                // creo l'hash e il salt della password
                var (hash, salt) = HashPassword(model.password);

                var newUser = new User
                {
                    Email = model.email,
                    PasswordHash = hash,
                    PasswordSalt = salt
                };

                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                return (newUser, null);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione dell'utente");
                return (null, "Errore durante la registrazione dell'utente");
            }

        }

        // metodo privato per generare hash e salt della password
        private (string hash, string salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            // converto la chiave generata casualamente e la uso come salt
            var salt = Convert.ToBase64String(hmac.Key);
            // metodo per calcolare l'hash combinadolo con il salt
            var hash = ComputedHash(password, hmac.Key);

            // restituisce il salt e l'hash
            return (hash, salt);
        }

        // Questo metodo calcola l'hash della password utilizzando l'algoritmo HMAC-SHA512
        // combinato con un salt specificato.
        private string ComputedHash (string password, byte[] salt)
        {
            // Crea un'istanza dell'algoritmo HMAC-SHA512 usando il salt fornito come chiave.
            using var hmac = new HMACSHA512(salt);

            // converte la password in un array di byte utilizzando la codifica
            // UTF-8
            // quindi calcola l'hash della password combinata con il salt
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Converte l'hash risultante (un array di byte) in una stringa
            // in formato base64 e la restituisce.
            // Questo formato è più adatto per la
            // memorizzazione o la trasmissione.
            return Convert.ToBase64String(hash);
        }

        // metodo privato per verifcare se la password fornita corrisponde
        // a quella memorizzata
        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // converto il salt memorizzato da una stringa base 64
            // a un array di byte
            var salt = Convert.FromBase64String(storedSalt);

            // calcolo l'hash della password fornita utilizzando il salt memorizzato
            var hash = ComputedHash(password, salt);

            // Confronta l'hash calcolato con l'hash memorizzato (storedHash).
            // Se gli hash corrispondono, la password è corretta e il metodo restituisce 'true'.
            // Altrimenti, restituisce 'false'.
            return storedHash == hash;
        }
    }
}

