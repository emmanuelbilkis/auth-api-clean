using System.Diagnostics;

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
                await _next(context);
            }
            catch (Exception ex)
            {
                // Logs generales
                _logger.LogError(ex, "Ocurrió una excepción no manejada.");
                _logger.LogError("Mensaje: {Message}", ex.Message);

                // StackTrace: lugar exacto donde explotó
                var methodInfo = new StackTrace(ex, true)
                                    .GetFrames()?
                                    .FirstOrDefault(f => f.GetMethod()?.DeclaringType?.Namespace?.StartsWith("TuProyecto") == true); // filtra por tu proyecto

                if (methodInfo != null)
                {
                    var method = methodInfo.GetMethod();
                    var file = methodInfo.GetFileName();
                    var line = methodInfo.GetFileLineNumber();

                    _logger.LogError("Método: {Method}", method.Name);
                    _logger.LogError("Clase: {Class}", method.DeclaringType?.FullName);
                    _logger.LogError("Archivo: {File}", file);
                    _logger.LogError("Línea: {Line}", line);
                }

                if (ex.InnerException != null)
                {
                    _logger.LogError("Excepción interna: {InnerMessage}", ex.InnerException.Message);
                    _logger.LogError("Stack interna: {InnerStack}", ex.InnerException.StackTrace);
                }

                _logger.LogError("HTTP {Method} {Path}", context.Request.Method, context.Request.Path);

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
