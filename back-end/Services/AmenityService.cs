using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class AmenityService : IAmenityService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AmenityService> _logger;

        public AmenityService(ApplicationDbContext dbContext, ILogger<AmenityService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Amenity> Create(Amenity newAmenity)
        {
            try
            {
                await _dbContext.Amenities.AddAsync(newAmenity);
                await _dbContext.SaveChangesAsync();
                return newAmenity;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Errore durante la creazione della comodità");
                throw new InvalidOperationException("Non è stato possibile creare la comodità, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore generico durante la creazione della comodità");
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var amenity = await _dbContext.Amenities
                    .FindAsync(id);
                if (amenity == null)
                {
                    throw new KeyNotFoundException($"Comodità con ID: {id} non trovata");
                }

                _dbContext.Amenities.Remove(amenity);
                await _dbContext.SaveChangesAsync();
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, $"Tentativo di eliminare una comodità non trovata, ID: {id}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Errore durante l'eliminazione della comodità nel database");
                throw new InvalidOperationException("Non è stato possibile eliminare la comodità, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore generico durante l'eliminazione della comodità");
                throw;
            }
        }

        public async Task<Amenity> Edit(int id, Amenity updateAmenity)
        {
            try
            {
                var existingAmenity = await _dbContext.Amenities
                    .FindAsync(id);
                if (existingAmenity == null)
                {
                    throw new KeyNotFoundException($"Comodità con ID: {id} non trovata");
                }

                existingAmenity.Name = updateAmenity.Name;
                existingAmenity.Description = updateAmenity.Description;

                await _dbContext.SaveChangesAsync();

                return existingAmenity;
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, $"Tentativo di modificare una comodità non trovata, ID: {id}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Errore durante la modifica della comodità nel database");
                throw new InvalidOperationException("Non è stato possibile modificare la comodità, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la modifica della comodità");
                throw;
            }
        }

        public async Task<List<Amenity>> GetAll()
        {
            try
            {
                var amenities = await _dbContext.Amenities.ToListAsync();
                return amenities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero di tutte le comodità");
                throw new InvalidOperationException("Non è stato possibile recuperare le comodità, riprovare più tardi.", ex);
            }
        }
    }
}

