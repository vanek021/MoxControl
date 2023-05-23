using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Controllers
{
    public class ProxmoxSettingController : Controller
    {
        private readonly ConnectProxmoxDbContext _connectProxmoxDbContext;

        public ProxmoxSettingController(ConnectProxmoxDbContext connectProxmoxDbContext)
        {
            _connectProxmoxDbContext = connectProxmoxDbContext;
        }

        public async Task<IActionResult> Index(long id)
        {
            var server = await _connectProxmoxDbContext.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id);

            if (server is null)
                return NotFound();

            var serverVm = new ProxmoxServerSettingViewModel()
            {
                Id = server.Id,
                BaseNode = server.BaseNode,
                BaseStorage = server.BaseStorage,
                Realm = server.Realm
            };

            return View("Index", serverVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProxmoxServerSettingViewModel viewModel)
        {
            var server = await _connectProxmoxDbContext.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if (server is null)
                return NotFound();

            server.BaseNode = viewModel.BaseNode;
            server.BaseStorage = viewModel.BaseStorage;
            server.Realm = viewModel.Realm;

            await _connectProxmoxDbContext.SaveChangesAsync();

            return RedirectToAction("Details", "Server", new { virtualizationSystem = VirtualizationSystem.Proxmox, id = viewModel.Id });
        }
    }
}
