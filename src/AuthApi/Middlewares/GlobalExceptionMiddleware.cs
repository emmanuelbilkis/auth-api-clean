namespace AuthApi.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Continúa con el pipeline
            }
            catch (Exception ex)
            {
                // Log detallado para desarrolladores
                _logger.LogError(ex, "Ocurrió una excepción no manejada.");

                _logger.LogError("Mensaje de la excepción: {Message}", ex.Message);
                
                // Si la excepción tiene una InnerException, también la logueamos
                if (ex.InnerException != null)
                {
                    _logger.LogError("Excepción interna: {InnerExceptionMessage}", ex.InnerException.Message);
                    _logger.LogError("Pila de llamadas de la excepción interna: {InnerExceptionStackTrace}", ex.InnerException.StackTrace);
                }

                _logger.LogError("Método HTTP: {Method}", context.Request.Method);
                _logger.LogError("Ruta: {Path}", context.Request.Path);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Ocurrió un error inesperado. Por favor, intente más tarde."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
