using System.Net;
using System.Text.Json;

namespace EMS.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await WriteErrorResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await WriteErrorResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (NotImplementedException ex)
            {
                await WriteErrorResponse(
                    context,
                    HttpStatusCode.NotImplemented,
                    ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await WriteErrorResponse(
                    context,
                    HttpStatusCode.Unauthorized,
                    ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await WriteErrorResponse(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                await WriteErrorResponse(
                    context,
                    HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private static async Task WriteErrorResponse(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ApiErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }

    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
