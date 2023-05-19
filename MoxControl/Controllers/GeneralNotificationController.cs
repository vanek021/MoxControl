using Microsoft.AspNetCore.Mvc;
using MoxControl.Infrastructure.Services;
using MoxControl.Services;

namespace MoxControl.Controllers
{
	public class GeneralNotificationController : Controller
	{
		private readonly GeneralNotificationService _generalNotificationService;

		private const int pageSize = 20;

		public GeneralNotificationController(GeneralNotificationService generalNotificationService)
		{
			_generalNotificationService = generalNotificationService;
		}
		
		public async Task<IActionResult> Index(int page = 1)
		{
			var notifications = await _generalNotificationService.GetPagedAsync(page, pageSize);
			return View(notifications);
		}

		public async Task<IActionResult> Details(long id)
		{
			var notification = await _generalNotificationService.GetByIdAsync(id);

			if (notification is null)
				return NotFound();

			return View(notification);
		}
	}
}
