using Microsoft.AspNetCore.Mvc;
using MoxControl.Services;

namespace MoxControl.Controllers
{
    public class TemplateController : Controller
    {
        private readonly TemplateService _templateService;

        public TemplateController(TemplateService templateService)
        {
            _templateService = templateService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
