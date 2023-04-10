using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Core.Extensions;
using MoxControl.Services;
using MoxControl.ViewModels.UserViewModels;

namespace MoxControl.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Settings()
        {
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var viewModel = await _userService.GetUserSettingsViewModelAsync(userId);

            if (viewModel is null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(UserSettingsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserSettings(viewModel);

                if (result)
                    return RedirectToAction(nameof(Settings));
                else
                    return BadRequest();
            }

            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            return View();
        }
    }
}
