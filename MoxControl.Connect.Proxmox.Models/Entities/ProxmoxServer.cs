using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Models.Entities
{
    public class ProxmoxServer : BaseServer
    {
        public ProxmoxServer()
        {
            VirtualizationSystem = VirtualizationSystem.Proxmox;
        }

        public string BaseNode { get; set; } = "pve";
        public string BaseStorage { get; set; } = "local";
        public string Realm { get; set; } = "pam";
        public new virtual List<ProxmoxMachine> Machines { get; set; } = new();
    }
}
