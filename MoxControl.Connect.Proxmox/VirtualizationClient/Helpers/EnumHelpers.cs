using MoxControl.Connect.Models.Enums;

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
