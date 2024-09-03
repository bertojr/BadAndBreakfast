using System;
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
    }
}

