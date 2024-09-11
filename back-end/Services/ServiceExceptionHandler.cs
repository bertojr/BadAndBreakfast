using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class ServiceExceptionHandler:IServiceExceptionHandler
	{
		private readonly ILogger<ServiceExceptionHandler> _logger;

		public ServiceExceptionHandler(ILogger<ServiceExceptionHandler> logger)
		{
			_logger = logger;
		}

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access");
                throw new UnauthorizedAccessException("Non sei autorizzato ad eseguire questa operazione");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Key not found");
                throw new KeyNotFoundException("Risorsa non trovata.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument");
                throw new ArgumentException(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error");
                throw new InvalidOperationException("Errore durante l'aggiornamento del database, riprova più tardi.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the operation.");
                throw new InvalidOperationException("Si è verificato un errore durante l'operazione, riprova più tardi.");
            }
        }

        public async Task ExecuteAsync(Func<Task> operation)
        {
            try
            {
                await operation();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access");
                throw new UnauthorizedAccessException("Non sei autorizzato ad eseguire questa operazione");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Key not found");
                throw new KeyNotFoundException("Risorsa non trovata.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument");
                throw new ArgumentException(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error");
                throw new InvalidOperationException("Errore durante l'aggiornamento del database, riprova più tardi.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the operation.");
                throw new InvalidOperationException("Si è verificato un errore durante l'operazione, riprova più tardi.");
            }
        }
    }
}

