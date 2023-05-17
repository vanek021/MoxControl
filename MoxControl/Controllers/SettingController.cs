using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Connect.Models.Enums;
using MoxControl.Services;
using MoxControl.ViewModels.SettingViewModels;

namespace MoxControl.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly SettingService _settingService;

        public SettingController(SettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            var settingIndexVm = await _settingService.GetSettingIndexViewModelAsync();

            return View(settingIndexVm);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var settingVm = await _settingService.GetGeneralSettingViewModelAsync(id);

            return View(settingVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GeneralSettingViewModel generalSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _settingService.UpdateGeneralSetting(generalSettingViewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            return View(generalSettingViewModel);
        }

        public async Task<IActionResult> VirtualizationSystem(VirtualizationSystem virtualizationSystem)
        {
            var connecSettingVm = await _settingService.GetConnectSettingViewModelAsync(virtualizationSystem);

            return View(connecSettingVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VirtualizationSystem(ConnectSettingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _settingService.UpdateConnectSettings(viewModel);

                if (result)
                    return RedirectToAction(nameof(VirtualizationSystem), new { virtualizationSystem = viewModel.VirtualizationSystem });
                else
                    return BadRequest();
            }

            return RedirectToAction(nameof(VirtualizationSystem), new { virtualizationSystem = viewModel.VirtualizationSystem });
        }
    }
}
