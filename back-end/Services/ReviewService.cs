using System.Security.Claims;
using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class ReviewService : IReviewService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceExceptionHandler _exceptionHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public ReviewService(ApplicationDbContext dbContext,
            IServiceExceptionHandler exceptionHandler,
            IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _dbContext = dbContext;
            _exceptionHandler = exceptionHandler;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        // creazione nuova recensione
        public async Task<Review> Create(Review newReview)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                // recupero la claim dell'utente loggato
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // converto userIdClaim in un intero
                var userId = userIdClaim != null ? int.Parse(userIdClaim) : (int?)null;

                // recupero l'utente
                var user = userId == null ? null : await _userService.GetById(userId);

                newReview.User = user == null ? null : user;

                await _dbContext.Reviews.AddAsync(newReview);
                await _dbContext.SaveChangesAsync();

                return newReview;
            });
        }

        // recupero tutte le recensioni
        public async Task<List<Review>> GetAll()
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                return await _dbContext.Reviews
                    .Include(r => r.User)
                    .ToListAsync();
            });
        }


        /*
        // eseguo la modifica di una recensione
        public async Task<Review> Update(int id, Review updateReview)
        {
            try
            {
                var existingReview = await _dbContext.Reviews
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.ReviewID == id);

                if (existingReview == null)
                {
                    throw new KeyNotFoundException($"Recensione con ID {id} non trovata");
                }

                existingReview.title = updateReview.title;
                existingReview.Description = updateReview.Description;
                existingReview.raiting = updateReview.raiting;

                _dbContext.Reviews.Update(existingReview);
                await _dbContext.SaveChangesAsync();

                return existingReview;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante l'aggiornamento della recensione");
                throw new InvalidOperationException($"Non è stato possibile aggiornare la recensione, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante l'aggiornamento della recensione");
                throw;
            }
        }*/
    }
}

