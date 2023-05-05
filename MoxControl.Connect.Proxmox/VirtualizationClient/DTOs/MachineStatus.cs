using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.VirtualizationClient.DTOs
{
    public class MachineStatus
    {
        [JsonProperty("cpus")]
        public string? CPUs { get; set; }

        [JsonProperty("running-machine")]
        public string? RunningMachine { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("vmid")]
        public long VMid { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("qmpstatus")]
        public string? Qmpstatus { get; set; }
    }
}
