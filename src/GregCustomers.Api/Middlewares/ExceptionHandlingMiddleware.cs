using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace GregCustomers.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            var (statusCode, title, detail) = MapException(ex);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            var problem = new
            {
                type = "about:blank",
                title,
                status = statusCode,
                detail,
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
    
    private static string ResolveUniqueMessage(SqlException ex)
    {
        var msg = ex.Message;

        if (msg.Contains("UX_Clients_Email", StringComparison.OrdinalIgnoreCase))
            return "Email already exists.";

        if (msg.Contains("UX_Addresses_Client_Street", StringComparison.OrdinalIgnoreCase))
            return "Address already exists for this client.";

        return "A unique constraint was violated.";
    }

    private static (int Status, string Title, string Detail) MapException(Exception ex)
    {
        return ex switch
        {
            // Email duplicado (unique index)
            SqlException sqlEx when sqlEx.Number is 2601 or 2627
                => ((int)HttpStatusCode.Conflict, "Conflict", ResolveUniqueMessage(sqlEx)),
            ArgumentException arg => ((int)HttpStatusCode.BadRequest, "Bad Request", arg.Message),
            KeyNotFoundException notFound => (StatusCodes.Status404NotFound, "Not Found", notFound.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error", "An unexpected error occurred.")
        };
    }
}