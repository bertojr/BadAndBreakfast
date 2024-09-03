using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class CrudService<T> : ICrudService<T> where T : class
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CrudService<T>> _logger;

        public CrudService(ApplicationDbContext dbContext, ILogger<CrudService<T>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<T> Create(T newEntity)
        {
            try
            {
                await _dbContext.Set<T>().AddAsync(newEntity);
                await _dbContext.SaveChangesAsync();
                return newEntity;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante la creazione di {typeof(T).Name}");
                throw new InvalidOperationException($"Non è stato possibile creare {typeof(T).Name}, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante la creazione di {typeof(T).Name}");
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var entity = await _dbContext.Set<T>()
                    .FindAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"{typeof(T).Name} con ID: {id} non trovata");
                }

                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, $"Tentativo di eliminare {typeof(T).Name} non trovata, ID: {id}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante l'eliminazione {typeof(T).Name} nel database");
                throw new InvalidOperationException($"Non è stato possibile eliminare {typeof(T).Name}, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante l'eliminazione {typeof(T).Name}");
                throw;
            }
        }

        public async Task<T> Edit(int id, T updateEntity)
        {
            try
            {
                var existingEntity = await _dbContext.Set<T>()
                    .FindAsync(id);
                if (existingEntity == null)
                {
                    throw new KeyNotFoundException($"{typeof(T).Name} con ID: {id} non trovato");
                }

                // Recupera le proprietà chiave
                var keyProperties = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;

                // Per ogni proprietà che non è una chiave, copia il valore
                foreach (var property in _dbContext.Entry(existingEntity).Properties)
                {
                    if (!keyProperties.Any(kp => kp.Name == property.Metadata.Name))
                    {
                        var newValue = _dbContext.Entry(updateEntity).Property(property.Metadata.Name).CurrentValue;
                        property.CurrentValue = newValue;
                    }
                }
                await _dbContext.SaveChangesAsync();

                return existingEntity;
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, $"Tentativo di modificare {typeof(T).Name} non trovato, ID: {id}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante la modifica {typeof(T).Name} nel database");
                throw new InvalidOperationException($"Non è stato possibile modificare {typeof(T).Name}, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante la modifica {typeof(T).Name}");
                throw;
            }
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero di tutte le {typeof(T).Name}");
                throw new InvalidOperationException($"Non è stato possibile recuperare le {typeof(T).Name}, riprovare più tardi.", ex);
            }
        }
    }
}

