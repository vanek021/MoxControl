using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Connect.Models.Enums;
using MoxControl.Services;
using MoxControl.ViewModels.ServerViewModels;

namespace MoxControl.Controllers
{
    [Authorize]
    public class ServerController : Controller
    {
        private readonly ServerService _serverService;

        public ServerController(ServerService serverService)
        {
            _serverService = serverService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _serverService.GetServerIndexViewModelAsync();
            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new ServerViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serverService.CreateAsync(viewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            return View(viewModel);
        }

        public IActionResult Edit(long id, VirtualizationSystem virtualizationSystem)
        {
            var serverViewModel = _serverService.GetServerViewModelAsync(id, virtualizationSystem);

            if (serverViewModel is null)
                return NotFound();

            return View(serverViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serverService.UpdateAsync(viewModel);

                if (result)
                    RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            return View(viewModel);
        }
    }
}
