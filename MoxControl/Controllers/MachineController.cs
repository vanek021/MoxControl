using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Enums;
using MoxControl.Services;
using MoxControl.ViewModels.MachineViewModels;

namespace MoxControl.Controllers
{
    [Authorize]
    public class MachineController : Controller
    {
        private readonly MachineService _machineService;

        public MachineController(MachineService machineService)
        {
            _machineService = machineService;
        }

        public async Task<IActionResult> Index()
        {
            var machineIndexViewModel = await _machineService.GetMachineIndexViewModelAsync();
            return View(machineIndexViewModel);
        }

        public async Task<IActionResult> Details(VirtualizationSystem virtualizationSystem, long id)
        {
            var viewModel = await _machineService.GetMachineDetailsViewModelAsync(virtualizationSystem, id);

            if (viewModel is null)
                return NotFound();

            return View(viewModel);
        }

        public async Task<IActionResult> Create(VirtualizationSystem virtualizationSystem, long serverId)
        {
            var machineCreateEditViewModel = await _machineService.GetMachineViewModelForCreateAsync(virtualizationSystem, serverId);
            return View(machineCreateEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MachineCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _machineService.CreateAsync(viewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            viewModel.Templates = await _machineService.GetTemplatesSelectListAsync();
            viewModel.Images = await _machineService.GetImagesSelectListAsync(viewModel.VirtualizationSystem, viewModel.ServerId);
            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(VirtualizationSystem virtualizationSystem, long id)
        {
            var viewModel = await _machineService.GetMachineViewModelForEditAsync(virtualizationSystem, id);

            if (viewModel is null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MachineCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _machineService.UpdateAsync(viewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            viewModel.Templates = await _machineService.GetTemplatesSelectListAsync();
            viewModel.Images = await _machineService.GetImagesSelectListAsync(viewModel.VirtualizationSystem, viewModel.ServerId);
            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<MachineHealthModel>> GetMachineHealth(VirtualizationSystem virtualizationSystem, long id)
        {
            var machineHealthModel = await _machineService.GetMachineHealthModelAsync(virtualizationSystem, id);

            if (machineHealthModel is null)
                return NotFound();

            return machineHealthModel;
        }

        [HttpPost]
        public async Task<IActionResult> TurnOn(VirtualizationSystem virtualizationSystem, long id)
        {
            var result = await _machineService.TurnOnMachine(virtualizationSystem, id);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> TurnOff(VirtualizationSystem virtualizationSystem, long id)
        {
            var result = await _machineService.TurnOffMachine(virtualizationSystem, id);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Reboot(VirtualizationSystem virtualizationSystem, long id)
        {
            var result = await _machineService.RebootMachine(virtualizationSystem, id);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> HardReboot(VirtualizationSystem virtualizationSystem, long id)
        {
            var result = await _machineService.HardRebootMachine(virtualizationSystem, id);

            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}
