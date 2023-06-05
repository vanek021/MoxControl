using Microsoft.AspNetCore.Mvc;
using MoxControl.Connect.Models.Enums;
using MoxControl.Services;

namespace MoxControl.Controllers
{
    public class SyncController : Controller
    {
        private readonly SyncService _syncService;

        public SyncController(SyncService syncService)
        {
            _syncService = syncService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _syncService.GetSyncViewModelAsync();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult StartTemplatesSync()
        {
            _syncService.StartTemplatesSync();

            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult StartImagesSync()
        {
            _syncService.StartImagesSync();

            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult StartMachinesSync()
        {
            _syncService.StartMachinesSync();

            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult StartMachineSync(VirtualizationSystem virtualizationSystem, long serverId)
        {
            _syncService.StartMachineSync(virtualizationSystem, serverId);

            return Ok(new { suceess = true });
        }
    }
}
