using Microsoft.AspNetCore.Mvc;
using MoxControl.Core.Extensions;
using MoxControl.Services;

namespace MoxControl.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> MarkAsViewed(string returnUrl)
        {
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var notifications = await _notificationService.GetActiveUserNotificationsAsync();

            await _notificationService.MarkAsViewedRangeAsync(notifications.Select(n => n.Id).ToList());

            return LocalRedirect(returnUrl);
        }
    }
}
