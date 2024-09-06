using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class UserService : IUserService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext dbContext,
            ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<User>> GetAll()
        {
            try
            {
                return await _dbContext.Users
                    .Include(u => u.Roles)
                    .Include(u => u.Reviews)
                    .Include(u => u.Bookings)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero di tutti gli utenti");
                throw new InvalidOperationException($"Non è stato possibile recuperare gli utenti, riprovare più tardi.", ex);
            }
        }

        public async Task<User> GetById(int id)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.Roles)
                    .Include(u => u.Reviews)
                    .Include(u => u.Bookings)
                    .FirstOrDefaultAsync(u => u.UserID == id);
                if(user == null)
                {
                    throw new KeyNotFoundException($"Utente con ID {id} non trovato.");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero dell'utente con ID {id}");
                throw new InvalidOperationException($"Non è stato possibile recuperare l'utente, riprovare più tardi.", ex);
            }
        }

        public async Task AddRoleToUser(int userId, int roleId)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.UserID == userId);

                if (user == null)
                {
                    throw new KeyNotFoundException($"Utente con ID {userId} non trovato.");
                }

                var role = await _dbContext.Roles
                    .FirstOrDefaultAsync(r => r.RoleID == roleId);

                if (role == null)
                {
                    throw new KeyNotFoundException($"Ruolo con ID {roleId} non trovato.");
                }

                if (!user.Roles.Contains(role))
                {
                    user.Roles.Add(role);
                    _dbContext.Users.Update(user);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante l'aggiornamento dell'utente con ID {userId}.");
                throw new InvalidOperationException($"Errore durante l'aggiornamento dell'utente, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante l'aggiunta del ruolo all'utente con ID {userId}.");
                throw;
            }
        }

        /*
        public async Task<(User user, string errorMessage)> Update(int id, User updateUser)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == updateUser.Email))
            {
                return (null, "Email già in uso");
            }

            try
            {
                var existingUser = await _dbContext.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(r => r.UserID == id);

                if(existingUser == null){
                    throw new KeyNotFoundException($"Utente con ID {id} non trovato");
                }

                existingUser.Name = updateUser.Name;
                existingUser.Email = updateUser.Email;
                existingUser.Cell = updateUser.Cell;
                existingUser.DateOfBirth = updateUser.DateOfBirth;
                existingUser.Nationally = updateUser.Nationally;
                existingUser.Gender = updateUser.Gender;
                existingUser.Country = existingUser.Country;
                existingUser.Address = updateUser.Address;
                existingUser.City = updateUser.City;
                existingUser.CAP = updateUser.CAP;

                _dbContext.Users.Update(existingUser);
                await _dbContext.SaveChangesAsync();

                return (existingUser, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione dell'utente");
                return (null, "Errore durante la registrazione dell'utente");
            }
        }*/
    }
}

