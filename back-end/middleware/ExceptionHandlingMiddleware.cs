using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.middleware
{
	public class ExceptionHandlingMiddleware
	{
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IServiceExceptionHandler _exceptionHandler;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IServiceExceptionHandler exceptionHandler)
        {
            _next = next;
            _logger = logger;
            _exceptionHandler = exceptionHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                DbUpdateException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var message =
                string.IsNullOrEmpty(ex.Message) ?
                GetDefaultErrorMessage(statusCode) :
                ex.Message;

            _logger.LogError(ex, "An error occurred during the operation");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new {
                message = message,
                details = context.RequestServices.GetService<IWebHostEnvironment>()
                .IsDevelopment() ? ex.ToString() : null
            };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        // Impostazione dei messaggi di errore predefiniti
        private string GetDefaultErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status401Unauthorized => "Non sei autorizzato ad esequire questa operazione",
                StatusCodes.Status404NotFound => "Risorsa non trovata",
                StatusCodes.Status400BadRequest => "Richiesta non valida",
                StatusCodes.Status500InternalServerError => "Errore durante l'aggiornamento del Database, riprova più tardi",
                _ => "Si è verificato l'errore durabte l'operazione, riprovare più tardi"
            };
        }
    }
}

