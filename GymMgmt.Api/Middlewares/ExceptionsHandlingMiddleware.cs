using GymMgmt.Api.Middlewares.Responses;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Exceptions;
using GymMgmt.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GymMgmt.Api.Middlewares
{
    public class ExceptionsHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionsHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionsHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string correlationId = Guid.NewGuid().ToString();

            // Log full exception details internally (safe)
            _logger.LogError(exception,
                "Error {CorrelationId}: {Message}",
                correlationId,
                exception.Message);

            context.Response.ContentType = "application/json";

            (int statusCode, ApiResponse<object> response) = exception switch
            {
                NotFoundException notFoundEx => (
                    StatusCodes.Status404NotFound,
                    ApiResponse<object>.Fail("ResourceNotFound", notFoundEx.Message, correlationId)
                ),

                FluentValidation.ValidationException validationEx => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.ValidationError(
                        "One or more validation errors occurred",
                        validationEx.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                        correlationId)
                ),

                BadRequestException badRequestEx => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.Fail("BadRequest", badRequestEx.Message, correlationId)
                ),

                IdentityValidationException identityValidationEx => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.ValidationError(
                        "One or more identity validation errors occurred",
                        new Dictionary<string, string[]>(identityValidationEx.Errors),
                        correlationId)
                ),

                ConflictException conflictEx => (
                    StatusCodes.Status409Conflict,
                    ApiResponse<object>.Conflict(conflictEx.Message, correlationId)
                ),

                UnauthorizedException unauthorizedException => (
                    StatusCodes.Status401Unauthorized,
                    ApiResponse<object>.Unauthorized(unauthorizedException.Message, correlationId)
                ),

                DomainException domainEx => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.Fail("BusinessRuleViolation", domainEx.Message, correlationId)
                ),

                InvalidTokenException => (
                    StatusCodes.Status401Unauthorized,
                    ApiResponse<object>.Unauthorized("Invalid token provided", correlationId)
                ),

                UserNotFoundException => (
                    StatusCodes.Status404NotFound,
                    ApiResponse<object>.NotFound("User not found", correlationId)
                ),

                DatabaseConnectionException => (
                    StatusCodes.Status503ServiceUnavailable,
                    ApiResponse<object>.ServerError("Database connection failed", correlationId)
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.Fail(
                        "ServerError",
                        _env.IsDevelopment()
                            ? new
                            {
                                ExceptionType = exception.GetType().Name,
                                ExceptionMessage = exception.Message
                                // StackTrace intentionally omitted for security
                            }
                            : "An unexpected error occurred.",
                        correlationId)
                )
            };

            context.Response.StatusCode = statusCode;
            context.Response.Headers["X-Correlation-ID"] = correlationId;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionsHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsHandlingMiddleware>();
        }
    }
}
