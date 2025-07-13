using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

            // Add CORS headers to error response
            context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:4200");
            context.Response.Headers.Append("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");
            context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                Message = "Internal Server Error",
                Details = ex.Message
            }));
        }
    }
}
