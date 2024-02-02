using MongoDB.Driver;
using System.Net;

/// <summary>
/// Middleware for handling exceptions globally across the application
/// </summary>
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
        // Handle 401 Unauthorized Forbidden
        if (context.Response.StatusCode == 401 && context.Response.ContentLength == null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Zugriff verweigert: Sie haben keine Berechtigung.",
                StatusCode = context.Response.StatusCode
            });
        }
        // Handle 403 Unterminated Forbidden
        if (context.Response.StatusCode == 403 && context.Response.ContentLength == null)
           {
               await context.Response.WriteAsJsonAsync(new
               {
                   Message = "Zugriff verweigert: Sie haben keine Berechtigung.",
                   StatusCode = context.Response.StatusCode
               });
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        string message = exception.Message;
        int statusCode = (int)HttpStatusCode.InternalServerError;

        if (exception is MongoCommandException mongoCommandException)
        {
            message = "Ein MongoDB-Fehler ist aufgetreten: " + mongoCommandException.Message;
            statusCode = (int)HttpStatusCode.BadRequest;
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(new
        {
            Message = message,
            StatusCode = statusCode
        });
    }
}
