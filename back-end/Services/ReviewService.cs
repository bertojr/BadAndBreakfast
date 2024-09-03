using System;
using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class ReviewService : IReviewService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(ApplicationDbContext dbContext,
            ILogger<ReviewService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Review> Create(int? userId, Review newReview)
        {
            try
            {
                // recupero l'utente
                var user = await _dbContext.Users.FindAsync(userId);

                newReview.User = user;

                await _dbContext.Reviews.AddAsync(newReview);
                await _dbContext.SaveChangesAsync();

                return newReview;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante la creazione della recensione");
                throw new InvalidOperationException($"Non è stato possibile creare la recensione, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante la creazione della recensione");
                throw;
            }
        }

        public async Task<List<Review>> GetAll()
        {
            try
            {
                return await _dbContext.Reviews
                    .Include(r => r.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero di tutte le recensioni");
                throw new InvalidOperationException($"Non è stato possibile recuperare le recensioni, riprovare più tardi.", ex);
            }
        }

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
        }
    }
}

