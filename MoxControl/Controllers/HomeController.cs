using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Models;
using System.Diagnostics;

namespace MoxControl.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}