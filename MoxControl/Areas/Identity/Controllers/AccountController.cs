using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoxControl.Areas.Identity.ViewModels;
using MoxControl.Infrastructure.Configurations;
using MoxControl.Infrastructure.Services;
using MoxControl.Models.Entities;
using static MoxControl.Infrastructure.Services.LdapService;

namespace MoxControl.Areas.Identity.Controllers
{
    [Area("Identity")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly MoxControlUserManager _moxControlUserManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(MoxControlUserManager moxControlUserManager, SignInManager<User> signInManager)
        {
            _moxControlUserManager = moxControlUserManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var model = new LoginViewModel();

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _moxControlUserManager.LdapSignInAsync(model.Login, model.Password, model.RememberMe);

                if (result)
                    return LocalRedirect(returnUrl);

                ModelState.AddModelError("Login", "Неудачная попытка входа. Проверьте введенные данные и повторите попытку.");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
