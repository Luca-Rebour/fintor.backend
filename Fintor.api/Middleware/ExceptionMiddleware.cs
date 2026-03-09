using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain.Exceptions;
using System.Security.Authentication;

namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Excepción no controlada");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;
            string? code = null;

            switch (ex)
            {
                case ValidationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    code = "VALIDATION_ERROR";
                    break;

                case DomainException domainEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = domainEx.Message;
                    code = domainEx.Code;
                    break;

                case UnauthorizedAccessException:
                case InvalidCredentialException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = "Credenciales inválidas.";
                    code = "UNAUTHORIZED";
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Ocurrió un error inesperado.";
                    code = "INTERNAL_ERROR";
                    break;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                statusCode,
                message,
                code
            });
        }
    }
}