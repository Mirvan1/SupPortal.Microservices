using System.Net;
using System.Text.Json;

namespace SupPortal.UserService.API.Extension;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            _logger.LogInformation($"Coming request:{httpContext?.Request?.Path}\t Query:{JsonSerializer.Serialize(httpContext?.Request?.Query)} IP:{httpContext.Connection.RemoteIpAddress}");
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      
        var errorDto = JsonSerializer.Serialize(new ErrorDetail(
             exception.Message + " " + exception.InnerException
       ) );
        _logger.LogError(errorDto);

        await context.Response.WriteAsync(errorDto);
    }
}

public class ErrorDetail
{
    public ErrorDetail(string _ErrorMessage)
    {
        ErrorMessage = _ErrorMessage;
        isSuccess = false;
    }
    private bool isSuccess { get; set; } = false;
    public string ErrorMessage { get; set; }
}