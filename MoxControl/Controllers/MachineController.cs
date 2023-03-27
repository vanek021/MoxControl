using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Services;

namespace MoxControl.Controllers
{
    public class MachineController : Controller
    {
        private readonly MachineService _machineService;

        public MachineController(MachineService machineService)
        {
            _machineService = machineService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(long id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
