using Microsoft.AspNetCore.Http;

namespace MoxControl.Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static string? GetUsername(this HttpContext? httpContext)
        {
            if (httpContext is null)
                return null;

            return httpContext?.User?.Identity?.Name;
        }
    }
}
