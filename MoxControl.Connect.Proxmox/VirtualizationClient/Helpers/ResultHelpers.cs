using Corsinvest.ProxmoxVE.Api;
using Newtonsoft.Json;

namespace MoxControl.Connect.Proxmox.VirtualizationClient.Helpers
{
    public static class ResultHelpers
    {
        public static string GetUniqueTaskId(this Result result)
        {
            return ((string)JsonConvert
                .SerializeObject(result.Response.data, Formatting.Indented))
                .TrimStart('"').TrimEnd('"');
        }
    }
}
