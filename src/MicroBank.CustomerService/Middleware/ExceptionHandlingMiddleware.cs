using System.Net;
using System.Text.Json;
using MicroBank.CustomerService.Exceptions;

namespace MicroBank.CustomerService.Middleware;

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
            var response = context.Response;
            response.ContentType = "application/json";
            var status = ex switch
            {
                CustomerNotFoundException => HttpStatusCode.NotFound,
                ApiException apiEx => (HttpStatusCode)apiEx.StatusCode,
                _ => HttpStatusCode.InternalServerError
            };

            response.StatusCode = (int)status;

            var payload = JsonSerializer.Serialize(new
            {
                error = ex.Message,
                type = ex.GetType().Name,
                traceId = context.TraceIdentifier
            });

            await response.WriteAsync(payload);
        }
    }
}
