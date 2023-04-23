using Microsoft.AspNetCore.Http;
using PostCommon.Dto;

namespace PostCommon.Middleware;

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
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    public async Task HandleException(HttpContext context, Exception exception)
    {
        ResponseBase result = new ResponseBase();
		
        switch (exception)
        {
            case InvalidOperationException value:
                result.Message = "Validation error";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
            default:
                Console.WriteLine($"Unhandled error is thrown.");
                result.Message = "Internal server error.";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }
		
        Console.Error.WriteLine(exception);

        await context.Response.WriteAsJsonAsync(result);
    }
}