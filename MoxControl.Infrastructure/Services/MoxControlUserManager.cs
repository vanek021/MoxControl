using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoxControl.Core.Data;
using MoxControl.Core.Services.BucketStorage;
using MoxControl.Models.Entities;
using System.Text;

namespace MoxControl.Infrastructure.Services
{
    public class MoxControlUserManager : BaseUserManager<User>
    {
        private readonly LdapService _ldapService;
        private readonly SignInManager<User> _signInManager;
        private readonly IBucket _bucket;
        private readonly IDataProtector _dataProtector;
        public MoxControlUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<BaseUserManager<User>> logger, LdapService ldapService,
            SignInManager<User> signInManager, IBucketStorageService bucketStorageService, IDataProtectionProvider provider)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _ldapService = ldapService;
            _signInManager = signInManager;

            _bucket = bucketStorageService.GetBucket("users");
            _dataProtector = provider.CreateProtector("password");
        }

        public async Task<bool> LdapSignInAsync(string userName, string password, bool rememberMe)
        {
            var user = await _ldapService.SearchUserInfoInADAync(userName, password);

            if (user != null)
            {
                WriteProtectedPasswordFile(user.NormalizedUserName, password);
                await _signInManager.SignInAsync(user, rememberMe);
                return true;
            }

            return false;
        }

        private void WriteProtectedPasswordFile(string? userId, string password)
        {
            var protectedPassword = _dataProtector.Protect(password);

            var passwordPath = Path.Combine($"user-{userId}", "data", "password");
            if (_bucket.ContainsObject(passwordPath))
                _bucket.DeleteObject(passwordPath);

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(protectedPassword));
            _bucket.WriteObject(passwordPath, stream);
        }

        public string GetUserPasswordFromProtectedFile(string? userId)
        {
            var passwordPath = Path.Combine($"user-{userId}", "data", "password");

            if (!_bucket.ContainsObject(passwordPath))
                throw new FileNotFoundException("Не удалось найти защищенный файл с паролем пользователя");

            var stream = new MemoryStream();

            _bucket.ReadObject(passwordPath, stream);

            var protectedPassword = Encoding.ASCII.GetString(stream.ToArray());

            var password = _dataProtector.Unprotect(protectedPassword);

            return password;
        }
    }
}
