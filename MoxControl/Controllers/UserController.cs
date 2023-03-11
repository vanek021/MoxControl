using Microsoft.AspNetCore.Mvc;
using MoxControl.Services;

namespace MoxControl.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        private const int pageSize = 20;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var userVms = await _userService.GetUserViewModelsAsync(page, pageSize);
            return View(userVms);
        }
    }
}
