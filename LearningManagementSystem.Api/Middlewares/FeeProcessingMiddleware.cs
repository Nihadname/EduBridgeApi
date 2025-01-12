using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities.Common;

namespace LearningManagementSystem.Api.Middlewares
{
    public class FeeProcessingMiddleware
    {
        private readonly RequestDelegate _next;

        public FeeProcessingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var excludedPaths = new[] { "/Fee/ProcessPayment", "/Fee/UploadImageOfBankTransfer" }; // Add payment-related endpoints here

            if (excludedPaths.Contains(context.Request.Path.Value, StringComparer.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst("UserId")?.Value; 
                var feeService = context.RequestServices.GetService<IFeeService>();

                if (!string.IsNullOrEmpty(userId) && feeService != null)
                {
                    Result<bool> isFeePaid = await feeService.IsFeePaid(userId);
                    if (!isFeePaid.Data)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Access denied. Fees are unpaid.");
                        return;
                    }
                }
            }

            // Proceed to the next middleware if authenticated or not checked
            await _next(context);
        }

    }
}
