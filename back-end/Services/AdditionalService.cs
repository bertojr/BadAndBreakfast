    using back_end.Interfaces;
    using back_end.DataModels;
    using Microsoft.EntityFrameworkCore;

    namespace back_end.Services
    {
	    public class AdditionalService : IAdditionalService
	    {
            private readonly ApplicationDbContext _dbContext;
            private readonly ILogger<AdditionalService> _logger;

		    public AdditionalService(ApplicationDbContext dbContext, ILogger<AdditionalService> logger)
		    {
                _dbContext = dbContext;
                _logger = logger;
		    }

            public async Task<DataModels.AdditionalService> Create(DataModels.AdditionalService newService)
            {
                try
                {
                    await _dbContext.AdditionalServices.AddAsync(newService);
                    await _dbContext.SaveChangesAsync();
                    return newService;
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError(dbEx, "Errore durante la creazione del servizio nel database");
                    throw new InvalidOperationException("Non è stato possibile creare il servizio, riprovare più tardi.", dbEx);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore generico durante la creazione del servizio");
                    throw;
                }
            }

            public async Task Delete(int id)
            {
                try
                {
                    var service = await _dbContext.AdditionalServices
                        .FindAsync(id);
                    if(service == null)
                    {
                        throw new KeyNotFoundException($"Servizio con ID: {id} non trovato");
                    }

                    _dbContext.AdditionalServices.Remove(service);
                    await _dbContext.SaveChangesAsync();
                }
                catch (KeyNotFoundException knfEx)
                {
                    _logger.LogWarning(knfEx, $"Tentativo di eliminare un servizio non trovato, ID: {id}");
                    throw;
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError(dbEx, "Errore durante l'eliminazione del servizio nel database");
                    throw new InvalidOperationException("Non è stato possibile eliminare il servizio, riprovare più tardi.", dbEx);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore generico durante l'eliminazione del servizio");
                    throw;
                }
            }

            public async Task<DataModels.AdditionalService> Edit(int id, DataModels.AdditionalService updateService)
            {
                try
                {
                    var existingService = await _dbContext.AdditionalServices
                        .FindAsync(id);
                    if (existingService == null)
                    {
                        throw new KeyNotFoundException($"Servizio con ID: {id} non trovato");
                    }

                    existingService.Name = updateService.Name;
                    existingService.Description = updateService.Description;
                    existingService.UnitPrice = updateService.UnitPrice;

                    await _dbContext.SaveChangesAsync();

                    return existingService;
                }
                catch (KeyNotFoundException knfEx)
                {
                    _logger.LogWarning(knfEx, $"Tentativo di modificare un servizio non trovato, ID: {id}");
                    throw;
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError(dbEx, "Errore durante la modifica del servizio nel database");
                    throw new InvalidOperationException("Non è stato possibile modificare il servizio, riprovare più tardi.", dbEx);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante la modifica del servizio");
                    throw;
                }
            }

            public async Task<List<DataModels.AdditionalService>> GetAll()
            {
                try
                {
                    var allService = await _dbContext.AdditionalServices.ToListAsync();
                    return allService;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante il recupero di tutti i servizi aggiuntivi");
                    throw new InvalidOperationException("Non è stato possibile recuperare i servizi, riprovare più tardi.", ex);
                }
            }
        }
    }

