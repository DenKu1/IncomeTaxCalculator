using IncomeTaxCalculator.API.ViewModels.Responses;
using System.Net;
using System.Text.Json;

namespace IncomeTaxCalculator.API.Middleware;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json; charset=UTF-8";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var content = ResponseViewModel.ErrorResponse(
            $"Internal Server Error: {exception.Message}; InnerException: {exception.InnerException?.Message}");

        await context.Response.WriteAsync(JsonSerializer.Serialize(content));
        await context.Response.Body.FlushAsync();
    }
}
