using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.VirtualizationClient.DTOs
{
    public class MachineList
    {
        public List<MachineListItem> Data { get; set; } = new();
    }

    public class MachineListItem
    {
        [JsonProperty("maxdisk")]
        public long HDDTotal { get; set; }

        [JsonProperty("disk")]
        public string? HDDUsed { get; set; }

        [JsonProperty("cpu")]
        public string? CPUUsed { get; set; }

        [JsonProperty("cpus")]
        public int CPUs { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("vmid")]
        public int VMid { get; set; }

        [JsonProperty("maxmem")]
        public long MemoryTotal { get; set; }

        [JsonProperty("mem")]
        public string? MemoryUsed { get; set; }

        [JsonProperty("template")]
        public int Template { get; set; }
    }
}
