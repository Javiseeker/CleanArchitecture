using CleanArchitecture.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace CleanArchitecture.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            DomainException domainEx => new ErrorResponse
            {
                Title = "Domain Error",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = domainEx.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            ArgumentException argEx => new ErrorResponse
            {
                Title = "Invalid Argument",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = argEx.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            KeyNotFoundException => new ErrorResponse
            {
                Title = "Resource Not Found",
                Status = (int)HttpStatusCode.NotFound,
                Detail = "The requested resource was not found.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            },
            _ => new ErrorResponse
            {
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred. Please try again later.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            }
        };

        response.StatusCode = errorResponse.Status;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonOptions);
        await response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Instance { get; set; }
}