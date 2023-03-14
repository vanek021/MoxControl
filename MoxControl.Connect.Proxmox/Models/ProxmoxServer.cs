using MoxControl.Connect.Enums;
using MoxControl.Connect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Models
{
    public class ProxmoxServer : BaseServer
    {
        public override VirtualizationSystem VirtualizationSystem { get; set; } = VirtualizationSystem.Proxmox;
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
