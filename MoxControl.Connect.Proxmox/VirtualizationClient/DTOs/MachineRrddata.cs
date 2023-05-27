using Newtonsoft.Json;

namespace MoxControl.Connect.Proxmox.VirtualizationClient.DTOs
{
    public class MachineRrddata
    {
        public List<MachineRrddataItem> Data { get; set; } = new();
    }

    public class MachineRrddataItem
    {
        [JsonProperty("maxdisk")]
        public string? HDDTotal { get; set; }

        [JsonProperty("disk")]
        public string? HDDUsed { get; set; }

        [JsonProperty("cpu")]
        public string? CPUUsed { get; set; }

        [JsonProperty("maxmem")]
        public string? MemoryTotal { get; set; }

        [JsonProperty("mem")]
        public string? MemoryUsed { get; set; }

        [JsonProperty("time")]
        public long DateTimeTicks { get; set; }
    }
}
