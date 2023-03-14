using Microsoft.AspNetCore.Mvc;
using MoxControl.Services;

namespace MoxControl.Controllers
{
    public class ServerController : Controller
    {
        private readonly ServerService _serverService;

        public ServerController(ServerService serverService)
        {
            _serverService = serverService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
