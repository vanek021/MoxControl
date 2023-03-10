using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoxControl.Areas.Identity.ViewModels;
using MoxControl.Infrastructure.Configurations;
using MoxControl.Infrastructure.Services;
using System.DirectoryServices.Protocols;

namespace MoxControl.Areas.Identity.Controllers
{
    [Area("Identity")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly LdapService _ldapService;
        private readonly ADConfig _adConfig;

        public AccountController(LdapService ldapService, IOptions<ADConfig> adConfig)
        {
            _ldapService = ldapService;
            _adConfig = adConfig.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var attributesToQuery = new string[] { "objectGUID", "sAMAccountName", "displayName", "mail", "whenCreated" };

            _ldapService.SearchInAD(_adConfig.Domain, _adConfig.Username, _adConfig.Password, LdapConstants.GroupsOU, string.Empty, SearchScope.Base, attributesToQuery);
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
