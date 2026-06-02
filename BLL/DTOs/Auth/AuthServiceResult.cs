using Microsoft.AspNetCore.Http;

namespace BLL.DTOs.Auth
{
    public class AuthServiceResult
    {
        public int StatusCode { get; set; }
        public object? Body { get; set; }
        public string? CreatedLocation { get; set; }

        public static AuthServiceResult Ok(object? body)
            => new() { StatusCode = StatusCodes.Status200OK, Body = body };

        public static AuthServiceResult Created(object? body, string location = "")
            => new() { StatusCode = StatusCodes.Status201Created, Body = body, CreatedLocation = location };

        public static AuthServiceResult BadRequest(object? body)
            => new() { StatusCode = StatusCodes.Status400BadRequest, Body = body };

        public static AuthServiceResult Unauthorized(object? body)
            => new() { StatusCode = StatusCodes.Status401Unauthorized, Body = body };

        public static AuthServiceResult Forbidden(object? body)
            => new() { StatusCode = StatusCodes.Status403Forbidden, Body = body };

        public static AuthServiceResult NotFound(object? body)
            => new() { StatusCode = StatusCodes.Status404NotFound, Body = body };

        public static AuthServiceResult Conflict(object? body)
            => new() { StatusCode = StatusCodes.Status409Conflict, Body = body };

        public static AuthServiceResult ServerError(object? body)
            => new() { StatusCode = StatusCodes.Status500InternalServerError, Body = body };
    }
}
