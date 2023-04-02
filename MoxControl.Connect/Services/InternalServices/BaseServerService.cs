using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Services.InternalServices
{
    public class BaseServerService
    {
        private readonly MoxControlUserManager _moxControlUserManager;

        public BaseServerService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _moxControlUserManager = scope.ServiceProvider.GetRequiredService<MoxControlUserManager>();
        }

        protected ServerCredentialsModel GetServerCredentials(BaseServer server, string? initiatorUsername = null)
        {
            string? userName = null;
            string? password = null;

            if (server.AuthorizationType == AuthorizationType.UserCredentials && !string.IsNullOrEmpty(initiatorUsername))
            {
                userName = initiatorUsername;
                password = _moxControlUserManager.GetUserPasswordFromProtectedFile(initiatorUsername);
            }

            if (server.AuthorizationType == AuthorizationType.RootCredentials)
            {
                userName = server.RootLogin;
                password = server.RootPassword;
            }

            if (userName is null || password is null)
                throw new ArgumentException("Не удалось найти данные авторизации сервера");

            return new ServerCredentialsModel()
            {
                Login = userName,
                Password = password
            };
        }
    }
}
