using Newtonsoft.Json;

namespace MoxControl.Connect.Proxmox.VirtualizationClient.DTOs
{
    public class Rrddata
    {
        public List<RrddataItem> Data { get; set; } = new();
    }

    public class RrddataItem
    {
        [JsonProperty("memtotal")]
        public string? MemoryTotal { get; set; }

        [JsonProperty("memused")]
        public string? MemoryUsed { get; set; }

        [JsonProperty("cpu")]
        public string? CPUUsed { get; set; }

        [JsonProperty("roottotal")]
        public string? HDDTotal { get; set; }

        [JsonProperty("rootused")]
        public string? HDDUsed { get; set; }

        [JsonProperty("time")]
        public long DateTimeTicks { get; set; }
    }
}
