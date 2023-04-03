using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Controllers
{
    public class ProxmoxSettingController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View("Index");
        }
    }
}
