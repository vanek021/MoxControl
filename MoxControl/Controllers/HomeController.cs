using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MoxControl.Models;
using MoxControl.Services;
using System.Diagnostics;

namespace MoxControl.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger, HomeService homeService)
        {
            _homeService = homeService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var homeIndexVm = await _homeService.GetHomeIndexViewModelAsync();
            return View(homeIndexVm);
        }

        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Visitor()
        {
            return RedirectToAction("VisitorLogin", "Account", new { area = "Identity" });
        }

        [AllowAnonymous]
        public IActionResult Error(string id)
        {
            int statusCode = int.Parse(id);
            var model = new ErrorViewModel()
            {
                ReturnUrl = "/Home/Index",
                StatusCode = statusCode,
                Description = ReasonPhrases.GetReasonPhrase(statusCode)
			};
            model.Title = $"Error {model.StatusCode}";
            return View(model);
        }
    }
}