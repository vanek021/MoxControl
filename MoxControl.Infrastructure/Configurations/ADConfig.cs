using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Infrastructure.Configurations
{
    public class ADConfig
    {
        public string Server { get; set; }
        public int Port { get; set; } = 389;
        public string Zone { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Users { get; set; } = string.Empty;
        public string Administrators { get; set; } = string.Empty;
    }
}
