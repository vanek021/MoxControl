using Corsinvest.ProxmoxVE.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.VirtualizationClient.DTOs
{
    public class CreateMachineResult
    {
        public CreateMachineResult(string uniqueTaskId, int vmId)
        {
            UniqueTaskId = uniqueTaskId;
            VmId = vmId;
        }

        public string UniqueTaskId { get; set; }
        public int VmId { get; set; }
    }
}
