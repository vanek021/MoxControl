using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoxControl.Core.Data;
using MoxControl.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Infrastructure.Services
{
    public class MoxControlUserManager : BaseUserManager<User>
    {
        private readonly LdapService _ldapService;
        private readonly SignInManager<User> _signInManager;
        public MoxControlUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<BaseUserManager<User>> logger, LdapService ldapService,
            SignInManager<User> signInManager)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _ldapService = ldapService;
            _signInManager = signInManager;
        }

        public async Task<bool> LdapSignInAsync(string userName, string password, bool rememberMe)
        {
            var user = await _ldapService.SearchUserInfoInADAync(userName, password);

            if (user != null)
            {
                await _signInManager.SignInAsync(user, rememberMe);
                return true;
            }

            return false;
        }
    }
}
