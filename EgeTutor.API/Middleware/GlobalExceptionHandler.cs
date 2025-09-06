using EgeTutor.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;

namespace EgeTutor.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var (statusCode, title, details) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = details,
                Instance = httpContext.Request.Path
            };

            // Добавляем дополнительные детали для ValidationException
            if (exception is ValidationException validationException && validationException.Errors != null)
            {
                problemDetails.Extensions["errors"] = validationException.Errors;
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
        private static (int StatusCode, string Title, string Details) MapException(Exception exception)
        {
            return exception switch
            {
                NotFoundException =>
                    ((int)HttpStatusCode.NotFound, "Not Found", exception.Message),

                ValidationException =>
                    ((int)HttpStatusCode.BadRequest, "Validation Error", exception.Message),

                ArgumentException or ArgumentNullException =>
                    ((int)HttpStatusCode.BadRequest, "Bad Request", exception.Message),

                UnauthorizedAccessException =>
                    ((int)HttpStatusCode.Unauthorized, "Unauthorized", exception.Message),

                DbUpdateException dbUpdateException =>
                    HandleDbUpdateException(dbUpdateException),

                _ =>
                    ((int)HttpStatusCode.InternalServerError, "Internal Server Error",
                     "An unexpected error occurred. Please try again later.")
            };
        }
        private static (int StatusCode, string Title, string Details) HandleDbUpdateException(DbUpdateException exception)
        {
            // Обработка foreign key violation
            if (exception.InnerException is PostgresException postgresException &&
                postgresException.SqlState == "23503")
            {
                return ((int)HttpStatusCode.BadRequest, "Foreign Key Violation",
                        "The referenced entity does not exist.");
            }

            // Обработка unique constraint violation
            if (exception.InnerException is PostgresException postgresEx &&
                postgresEx.SqlState == "23505")
            {
                return ((int)HttpStatusCode.Conflict, "Duplicate Entry",
                        "An entity with this key already exists.");
            }

            return ((int)HttpStatusCode.InternalServerError, "Database Error",
                    "An error occurred while saving data.");
        }
    }
}
