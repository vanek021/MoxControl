using MoxControl.Connect.Interfaces;
using Corsinvest.ProxmoxVE.Api;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using MoxControl.Connect.Proxmox.VirtualizationClient.DTOs;

namespace MoxControl.Connect.Proxmox
{
    public class ProxmoxVirtualizationClient : IVirtualizationSystemClient
    {
        private readonly string _baseNode;
        private readonly PveClient _pveClient;

        public ProxmoxVirtualizationClient(string host, int port, string login, string password, string baseNode = "pve")
        {
            _pveClient = new PveClient(host, port);
            _pveClient.Login(login, password).GetAwaiter().GetResult();
            _baseNode = baseNode;
        }

        public async Task<List<RrddataItem>> GetServerRrdata(string timeframe = "hour", string cf = "AVERAGE")
        {
            return await GetNodeRrdata(_baseNode, timeframe, cf);
        }

        public async Task<List<RrddataItem>> GetMachineRrdata(string nodeName, string timeFrame = "hour", string cf = "AVERAGE")
        {
            return await GetNodeRrdata(nodeName, timeFrame, cf);
        }

        private async Task<List<RrddataItem>> GetNodeRrdata(string nodeName, string timeFrame = "hour", string cf = "AVERAGE")
        {
            var rrddata = await _pveClient.Nodes[nodeName].Rrddata.Rrddata(timeFrame, cf);
            var stringResponse = JsonConvert.SerializeObject(rrddata.Response.data, Formatting.Indented);

            List<RrddataItem> rrddataItems = JsonConvert.DeserializeObject<List<RrddataItem>>(stringResponse);

            rrddataItems = rrddataItems.
                Where(x => !string.IsNullOrEmpty(x.HDDUsed) && !string.IsNullOrEmpty(x.HDDTotal)
                    && !string.IsNullOrEmpty(x.MemoryUsed) && !string.IsNullOrEmpty(x.MemoryTotal)
                    && !string.IsNullOrEmpty(x.CPUUsed))
                .OrderByDescending(x => x.DateTimeTicks)
                .ToList();

            return rrddataItems;
        }
    }
}