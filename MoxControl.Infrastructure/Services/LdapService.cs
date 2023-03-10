using Microsoft.Extensions.Options;
using MoxControl.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Infrastructure.Services
{
    public class LdapService
    {
        private readonly ADConfig _adConfig;

        public LdapService(IOptions<ADConfig> adConfig)
        {
            _adConfig = adConfig.Value;
        }

        public SearchResponse SearchInAD(string domainForAD, string username, string password, string targetOU,
            string query, SearchScope scope, params string[] attributeList)
        {
            // on Windows the authentication type is Negotiate, so there is no need to prepend
            // AD user login with domain. On other platforms at the moment only
            // Basic authentication is supported
            var authType = AuthType.Basic;
            // also can fail on non AD servers, so you might prefer
            // to just use AuthType.Basic everywhere
            if (!OperatingSystem.IsWindows())
            {
                authType = AuthType.Basic;
                username = OperatingSystem.IsWindows()
                    ? username
                    // this might need to be changed to your actual AD domain value
                    : $"{domainForAD}\\{username}";
            }

            // depending on LDAP server, username might require some proper wrapping
            // instead(!) of prepending username with domain
            //username = $"uid={username},CN=Users,DC=subdomain,DC=domain,DC=zone";

            //var connection = new LdapConnection(ldapServer)
            var connection = new LdapConnection(
                new LdapDirectoryIdentifier(_adConfig.Server, 389)
                )
            {
                AuthType = authType,
                Credential = new(_adConfig.Username, _adConfig.Password)
            };
            // the default one is v2 (at least in that version), and it is unknown if v3
            // is actually needed, but at least Synology LDAP works only with v3,
            // and since our Exchange doesn't complain, let it be v3
            connection.SessionOptions.ProtocolVersion = 3;

            // this is for connecting via LDAPS (636 port). It should be working,
            // according to https://github.com/dotnet/runtime/issues/43890,
            // but it doesn't (at least with Synology DSM LDAP), although perhaps
            // for a different reason
            //connection.SessionOptions.SecureSocketLayer = true;

            connection.Bind();

            var request = new SearchRequest(targetOU, query, scope, attributeList);

            return (SearchResponse)connection.SendRequest(request);
        }
    }

    public static class LdapConstants
    {
        public const string UsersOU = "users";
        public const string GroupsOU = "groups";
    }
}
