using Microsoft.Extensions.Options;
using MoxControl.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
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
    }
}
