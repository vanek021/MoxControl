using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MoxControl.Core.Models;
using MoxControl.Infrastructure.Configurations;
using MoxControl.Models.Entities;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Infrastructure.Services
{
    public class LdapService
    {
        private readonly ADConfig _adConfig;
        private readonly RoleManager<BaseRole> _roleManager;

        public LdapService(IOptions<ADConfig> adConfig, RoleManager<BaseRole> roleManager)
        {
            _roleManager = roleManager;
            _adConfig = adConfig.Value;
        }

        public User? SearchUserInfoInAD(string username, string password)
        {
            var connection = new LdapConnection() { SecureSocketLayer = false };

            connection.Connect(_adConfig.Server, _adConfig.Port);
            connection.Bind(_adConfig.Username, _adConfig.Password);

            var searchFilter = string.Format(_adConfig.SearchFilter, username);

            var attributes = new[] 
            { 
                LDAPAttributeConstants.GivenName,
                LDAPAttributeConstants.Sn,
                LDAPAttributeConstants.UserName,
                LDAPAttributeConstants.GidNumber
            };

            var groupAttributes = new[]
            {
                LDAPAttributeConstants.GidNumber,
                LDAPAttributeConstants.Cn
            };

            var userResult = connection.Search(_adConfig.SearchBase, LdapConnection.ScopeSub, searchFilter, attributes, false);
            var groupsResult = connection.Search(_adConfig.SearchBase, LdapConnection.ScopeSub, string.Empty, groupAttributes, false);

            try
            {
                var user = userResult.Next();
                if (user != null)
                {
                    connection.Bind(user.Dn, password);

                    if (connection.Bound)
                    {
                        var appUser = new User()
                        {
                            FirstName = user.GetAttribute(LDAPAttributeConstants.GivenName).StringValue,
                            LastName = user.GetAttribute(LDAPAttributeConstants.Sn).StringValue,
                            UserName = user.GetAttribute(LDAPAttributeConstants.UserName).StringValue
                        };

                        var gidNumber = user.GetAttribute(LDAPAttributeConstants.GidNumber).StringValue;
                    }
                }
            }
            catch
            {
                throw new Exception("Login failed.");
            }

            connection.Disconnect();
            return null;
        }
        public static class LdapOUConstants
        {
            public const string UsersOU = "users";
            public const string GroupsOU = "groups";
        }

        public static class LDAPAttributeConstants
        {
            public const string MemberOf = "memberOf";
            public const string SAMAccountName = "sAMAccountName";
            public const string DisplayName = "displayName";
            public const string ObjectGuid = "objectGUID";
            public const string Mail = "mail";
            public const string WhenCreated = "whenCreated";
            public const string GivenName = "givenName";
            public const string Sn = "sn";
            public const string Cn = "cn";
            public const string UserName = "uid";
            public const string GidNumber = "gidNumber";
        }
    }
}
