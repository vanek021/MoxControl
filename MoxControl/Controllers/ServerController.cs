using Microsoft.AspNetCore.Mvc;

namespace MoxControl.Controllers
{
    public class ServerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
