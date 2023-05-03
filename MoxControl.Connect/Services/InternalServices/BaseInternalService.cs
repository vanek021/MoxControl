﻿using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Models;
using MoxControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MoxControl.Connect.Services.InternalServices
{
    public abstract class BaseInternalService
    {
        protected readonly MoxControlUserManager _moxControlUserManager;

        public BaseInternalService(IServiceScopeFactory serviceScopeFactory) 
        {
            var scope = serviceScopeFactory.CreateScope();

            _moxControlUserManager = scope.ServiceProvider.GetRequiredService<MoxControlUserManager>();
        }

        protected virtual ServerCredentialsModel GetServerCredentials(BaseServer server, string? initiatorUsername = null)
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
