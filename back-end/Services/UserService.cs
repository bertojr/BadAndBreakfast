using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class UserService : IUserService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceExceptionHandler _exceptionHandler;

        public UserService(ApplicationDbContext dbContext,
            IServiceExceptionHandler exceptionHandler)
        {
            _dbContext = dbContext;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<List<User>> GetAll()
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                return await _dbContext.Users
                    .Include(u => u.Roles)
                    .Include(u => u.Reviews)
                    .Include(u => u.Bookings)
                    .ToListAsync();
            });
        }

        public async Task<User> GetById(int? id)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                var user = await _dbContext.Users
                    .Include(u => u.Roles)
                    .Include(u => u.Reviews)
                    .Include(u => u.Bookings)
                    .FirstOrDefaultAsync(u => u.UserID == id);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Utente con ID {id} non trovato.");
                }

                return user;
            });
        }

        public async Task<Role> AddRoleToUser(int userId, int roleId)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                // recupero l'utente
                var user = await _dbContext.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.UserID == userId);

                if (user == null)
                {
                    throw new KeyNotFoundException($"Utente con ID {userId} non trovato.");
                }

                // recupero il ruolo
                var role = await _dbContext.Roles
                    .FirstOrDefaultAsync(r => r.RoleID == roleId);

                if (role == null)
                {
                    throw new KeyNotFoundException($"Ruolo con ID {roleId} non trovato.");
                }

                if (user.Roles.Any(r => r.RoleID == roleId))
                {
                    throw new InvalidOperationException($"Utente già con ruolo {role.Name}");
                }

                user.Roles.Add(role);
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return role;
            });
        }

        public async Task RemoveRoleFromUser(int userId, int roleId)
        {
            await _exceptionHandler.ExecuteAsync(async () =>
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

                user.Roles.Remove(role);
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            });
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

