using Audisoft.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Audisoft.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            _logger.LogInformation("Request cancelada por el cliente: {Path}", httpContext.Request.Path);
            httpContext.Response.StatusCode = 499;
            return true;
        }

        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationEx => (
                (int)StatusCodes.Status400BadRequest,
                (string)"Error de validación",
                (string)string.Join(" | ", validationEx.Errors.Select(e => e.ErrorMessage))
            ),

            NotFoundException notFoundEx => (
                (int)StatusCodes.Status404NotFound,
                (string)"Recurso no encontrado",
                (string)notFoundEx.Message
            ),

            BusinessRuleException businessEx => (
                (int)StatusCodes.Status409Conflict,
                (string)"Regla de negocio violada",
                (string)businessEx.Message
            ),

            DbUpdateConcurrencyException => (
                (int)StatusCodes.Status409Conflict,
                (string)"Conflicto de concurrencia",
                (string)"El registro fue modificado o eliminado por otro proceso. Recarga los datos e intenta de nuevo."
            ),

            DbUpdateException => (
                (int)StatusCodes.Status409Conflict,
                (string)"Error al guardar los datos",
                (string)"No se pudo completar la operación: revisa que las referencias (Estudiante, Profesor, etc.) existan y sean válidas."
            ),

            _ => (
                (int)StatusCodes.Status500InternalServerError,
                (string)"Error interno del servidor",
                (string)(_environment.IsDevelopment() ? exception.Message : "Ocurrió un error inesperado.")
            )
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Excepción no controlada en {Path}: {Message}", httpContext.Request.Path, exception.Message);
        }
        else if (statusCode == StatusCodes.Status409Conflict)
        {
            _logger.LogWarning(exception, "Conflicto en {Path}: {Message}", httpContext.Request.Path, exception.Message);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
