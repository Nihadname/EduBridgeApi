using LearningManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace LearningManagementSystem.Api.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while processing the request.");

               await HandleExceptionAsync(httpContext, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            object errorResponse;

            switch (exception)
            {
                case CustomException customException:
                    response.StatusCode = customException.Code;
                    errorResponse = new
                    {
                        Message = customException.Message,
                        Errors = customException.Errors
                    };
                    break;

                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse = new
                    {
                        Message = "An unexpected error occurred.",
                        Errors = new Dictionary<string, string>()
                    };
                    break;
            }

            await response.WriteAsJsonAsync(errorResponse);
        }
    }
}
