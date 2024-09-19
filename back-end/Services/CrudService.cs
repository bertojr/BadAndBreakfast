using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class CrudService<T> : ICrudService<T> where T : class
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceExceptionHandler _exceptionHandler;

        public CrudService(ApplicationDbContext dbContext,
            IServiceExceptionHandler exceptionHandler)
        {
            _dbContext = dbContext;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<T> Create(T newEntity)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                await _dbContext.Set<T>().AddAsync(newEntity);
                await _dbContext.SaveChangesAsync();
                return newEntity;
            });
        }

        public async Task Delete(int id)
        {
            await _exceptionHandler.ExecuteAsync(async () =>
            {
                var entity = await _dbContext.Set<T>()
                    .FindAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"{typeof(T).Name} con ID: {id} non trovata");
                }

                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            });
        }

        public async Task<T> Edit(int id, T updateEntity)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
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
            });
        }

        public async Task<List<T>> GetAll()
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                return await _dbContext.Set<T>().ToListAsync();
            });
        }

        public async Task<T> GetById(int id)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);

                if (entity == null)
                {
                    throw new KeyNotFoundException($"{typeof(T).Name} con ID {id} non trovata");
                }

                return entity;
            });
        }
    }
}

