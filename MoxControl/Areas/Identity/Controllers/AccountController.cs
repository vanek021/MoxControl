using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Areas.Identity.ViewModels;
using MoxControl.Infrastructure.Services;

namespace MoxControl.Areas.Identity.Controllers
{
    [Area("Identity")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly LdapService _ldapService;

        public AccountController(LdapService ldapService)
        {
            _ldapService = ldapService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var model = new LoginViewModel();

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
