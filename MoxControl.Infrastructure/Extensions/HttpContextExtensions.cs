using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
