using System.Net;
using System.Text.Json;
using MicroBank.AccountService.Exceptions;

namespace MicroBank.AccountService.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                AccountNotFoundException => (int)HttpStatusCode.NotFound,
                CustomerValidationException => (int)HttpStatusCode.BadRequest,
                InsufficientBalanceException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var payload = JsonSerializer.Serialize(new
            {
                error = ex.Message,
                type = ex.GetType().Name,
                traceId = context.TraceIdentifier
            });

            await context.Response.WriteAsync(payload);
        }
    }
}
