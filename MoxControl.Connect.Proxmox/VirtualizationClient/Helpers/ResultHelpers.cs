using Corsinvest.ProxmoxVE.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
