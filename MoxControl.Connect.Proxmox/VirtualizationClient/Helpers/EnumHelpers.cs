using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.VirtualizationClient.Helpers
{
    public static class EnumHelpers
    {
        public static MachineStatus GetMachineStatus(this string? proxmoxStatus)
        {
            return proxmoxStatus switch
            {
                "running" => MachineStatus.Running,
                "stopped" => MachineStatus.Stopped,
                _ => MachineStatus.Unknown,
            };
        }
    }
}
