using MoxControl.Infrastructure.Services;
using MoxControl.Services;

namespace MoxControl.Middlewares
{
    public static class GeneralNotificationsMiddleware
    {
        public static void UseDbLogs(this IApplicationBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                try
                {
                    await next.Invoke();
                }
                catch (Exception ex)
                {
                    var scope = builder.ApplicationServices.CreateScope();
                    var generalNotificationsService = scope.ServiceProvider.GetRequiredService<GeneralNotificationService>();

                    await generalNotificationsService.AddInternalServerErrorAsync(ex);

                    throw;
                }
            });
        }
    }
}
