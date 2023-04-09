using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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

        [AllowAnonymous]
        public IActionResult Error(string id)
        {
            int statusCode = Int32.Parse(id);
            var model = new ErrorViewModel()
            {
                ReturnUrl = "/Home/Index",
                StatusCode = statusCode,
            };

            model.Title = model.StatusCode switch
            {
                400 => "Error 400",
                401 => "Error 401",
                403 => "Error 403",
                404 => "Error 404",
                _ => "Error 500",
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Visitor()
        {
            return RedirectToAction("VisitorLogin", "Account", new { area = "Identity" });
        }
    }
}