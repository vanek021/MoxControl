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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MoxControl.Infrastructure.Services
{
    public class LdapService
    {
        private readonly ADConfig _adConfig;
        private readonly RoleManager<BaseRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly LdapConnectionOptions _connectionOptions;

        public LdapService(IOptions<ADConfig> adConfig, RoleManager<BaseRole> roleManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _adConfig = adConfig.Value;

            _connectionOptions = new LdapConnectionOptions();
        }

        public async Task<User?> SearchUserInfoInADAync(string username, string password)
        {
            var connection = GetBaseBindedConnection();

            var searchFilter = string.Format(_adConfig.SearchFilter, username);

            var attributes = LDAPAttributeHelpers.GetUserAttributes();

            var userResult = connection.Search(_adConfig.SearchBase, LdapConnection.ScopeSub, searchFilter, attributes, false);         

            try
            {
                var user = userResult.Next();
                if (user != null)
                {
                    connection.Bind(user.Dn, password);

                    if (connection.Bound)
                    {
                        var currentUser = await _userManager.FindByNameAsync(username);
                        var gidNumber = user.GetAttribute(LDAPAttributeConstants.GidNumber).StringValue;
                        var role = await GetRoleByGidNumberAsync(gidNumber);

                        if (currentUser == null)
                        {
                            currentUser = new User()
                            {
                                FirstName = user.GetAttribute(LDAPAttributeConstants.GivenName).StringValue,
                                LastName = user.GetAttribute(LDAPAttributeConstants.Sn).StringValue,
                                Email = $"{username}@mox.com",
                                UserName = username
                            };

                            currentUser.Roles.Add(new BaseUserRole() { RoleId = role.Id });

                            await _userManager.CreateAsync(currentUser, password);
                        }
                        
                        else
                        {
                            currentUser.FirstName = user.GetAttribute(LDAPAttributeConstants.GivenName).StringValue;
                            currentUser.LastName = user.GetAttribute(LDAPAttributeConstants.Sn).StringValue;
                            currentUser.Email = $"{username}@mox.com";

                            if (!await _userManager.IsInRoleAsync(currentUser, role.Name!))
                            {
                                await _userManager.RemoveFromRolesAsync(currentUser, await _userManager.GetRolesAsync(currentUser));
                                await _userManager.AddToRoleAsync(currentUser, role.Name!);
                            }

                            await _userManager.UpdateAsync(currentUser);
                        }

                        return currentUser;
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public async Task<BaseRole> GetRoleByGidNumberAsync(string gidNumber)
        {
            var roles = await GetAllRolesByLdapGroupsAsync();

            return roles.Where(x => x.Item1 == gidNumber).Select(x => x.Item2).First();
        }

        public async Task<List<(string, BaseRole)>> GetAllRolesByLdapGroupsAsync()
        {
            var connection = GetBaseBindedConnection();

            var groupAttributes = LDAPAttributeHelpers.GetGroupAttributes();

            var groupsResult = connection.Search(_adConfig.SearchBase, LdapConnection.ScopeSub, _adConfig.GroupSearchFilter, groupAttributes, false);
            var groups = groupsResult.ToList();

            var roles = new List<(string, BaseRole)>();

            foreach (var group in groups)
            {
                var groupName = group.GetAttribute(LDAPAttributeConstants.Cn).StringValue;
                var role = await _roleManager.FindByNameAsync(groupName);

                if (role == null)
                {
                    await _roleManager.CreateAsync(new BaseRole(groupName));
                    role = await _roleManager.FindByNameAsync(groupName);
                }

                roles.Add((group.GetAttribute(LDAPAttributeConstants.GidNumber).StringValue, role!));
            }

            return roles;
        }

        private LdapConnection GetBaseBindedConnection()
        {
            var connection = new LdapConnection(_connectionOptions);

            connection.Connect(_adConfig.Server, _adConfig.Port);
            connection.Bind(_adConfig.Username, _adConfig.Password);

            return connection;
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

        public static class LDAPAttributeHelpers
        {
            public static string[] GetGroupAttributes()
            {
                return new[]
                {
                    LDAPAttributeConstants.GidNumber,
                    LDAPAttributeConstants.Cn
                };
            }

            public static string[] GetUserAttributes()
            {
                return new[]
                {
                LDAPAttributeConstants.GivenName,
                LDAPAttributeConstants.Sn,
                LDAPAttributeConstants.UserName,
                LDAPAttributeConstants.GidNumber
                };
            }
        }
    }
}
