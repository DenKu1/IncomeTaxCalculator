using IncomeTaxCalculator.Domain.Exceptions;

namespace IncomeTaxCalculator.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (TaxBandOperationException ex)
            {
                _logger.LogWarning(ex, $"'{typeof(TaxBandOperationException).Name}");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unnkown expection. Please, check stack trace to debug issue");

                throw;
            }
        }
    }
}
