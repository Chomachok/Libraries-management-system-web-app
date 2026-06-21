using System.Net;
using System.Text.Json;

namespace LibrariesManagementSystem.Api.Middleware;

/// <summary>
/// Промежуточное ПО для глобальной обработки необработанных исключений.
/// Логирует ошибку и возвращает клиенту JSON с сообщением (и стектрейсом в режиме разработки).
/// </summary>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
{
    /// <summary>
    /// Обрабатывает HTTP-запрос, перехватывая все необработанные исключения.
    /// </summary>
    /// <param name="context">Контекст текущего HTTP-запроса.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Необработанное исключение");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                error = ex.Message,
                stackTrace = env.IsDevelopment() ? ex.StackTrace : null
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
