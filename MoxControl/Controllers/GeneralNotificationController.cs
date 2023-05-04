using Microsoft.AspNetCore.Mvc;
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

		public IActionResult Details(long id)
		{
			return View();
		}
	}
}
