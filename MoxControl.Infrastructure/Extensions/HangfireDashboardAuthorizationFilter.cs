using Hangfire.Dashboard;

namespace MoxControl.Infrastructure.Extensions
{
    public class HangfireDashboardAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
