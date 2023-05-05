﻿using MoxControl.Connect.Interfaces;
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

        public async Task<List<RrddataItem>> GetServerRrddata(string timeframe = "hour", string cf = "AVERAGE")
        {
            await GetNodeMachines();
            return await GetNodeRrdata(_baseNode, timeframe, cf);
        }

        public async Task<List<MachineRrddataItem>> GetMachineRrddata(int machineId, string timeFrame = "hour", string cf = "AVERAGE")
        {
            var vm = _pveClient.Nodes[_baseNode].Qemu[machineId];
            var rrddata = await vm.Rrddata.Rrddata(timeFrame, cf);
            var stringResponse = JsonConvert.SerializeObject(rrddata.Response.data, Formatting.Indented);

            return DeserializeMachineRrddata(stringResponse);
        }

        public async Task<MachineStatus> GetMachineStatus(int machineId)
        {
            var vm = _pveClient.Nodes[_baseNode].Qemu[machineId];
            var status = await vm.Status.Current.VmStatus();
            var stringResponse = JsonConvert.SerializeObject(status.Response.data, Formatting.Indented);

            return DeserializeMachineStatus(stringResponse);
        }

        public async Task<List<MachineListItem>> GetNodeMachines()
        {
            var vmList = await _pveClient.Nodes[_baseNode].Qemu.Vmlist();
            var stringResponse = JsonConvert.SerializeObject(vmList.Response.data, Formatting.Indented);

            return DeserializeVmList(stringResponse);
        }

        private async Task<List<RrddataItem>> GetNodeRrdata(string nodeName, string timeFrame = "hour", string cf = "AVERAGE")
        {
            var rrddata = await _pveClient.Nodes[nodeName].Rrddata.Rrddata(timeFrame, cf);
            var stringResponse = JsonConvert.SerializeObject(rrddata.Response.data, Formatting.Indented);

            return DeserializeRrddata(stringResponse);
        }

        #region Serializations
        private List<RrddataItem> DeserializeRrddata(dynamic? stringResponse)
        {
            List<RrddataItem> rrddataItems = JsonConvert.DeserializeObject<List<RrddataItem>>(stringResponse);

            rrddataItems = rrddataItems.
                Where(x => !string.IsNullOrEmpty(x.HDDUsed) && !string.IsNullOrEmpty(x.HDDTotal)
                    && !string.IsNullOrEmpty(x.MemoryUsed) && !string.IsNullOrEmpty(x.MemoryTotal)
                    && !string.IsNullOrEmpty(x.CPUUsed))
                .OrderByDescending(x => x.DateTimeTicks)
                .ToList();

            return rrddataItems;
        }

        private List<MachineRrddataItem> DeserializeMachineRrddata(dynamic? stringResponse)
        {
            List<MachineRrddataItem> rrddataItems = JsonConvert.DeserializeObject<List<MachineRrddataItem>>(stringResponse);

            rrddataItems = rrddataItems.
                Where(x => !string.IsNullOrEmpty(x.HDDUsed) && !string.IsNullOrEmpty(x.HDDTotal)
                    && !string.IsNullOrEmpty(x.MemoryUsed) && !string.IsNullOrEmpty(x.MemoryTotal)
                    && !string.IsNullOrEmpty(x.CPUUsed))
                .OrderByDescending(x => x.DateTimeTicks)
                .ToList();

            return rrddataItems;
        }

        private MachineStatus DeserializeMachineStatus(dynamic? stringResponse)
        {
            return JsonConvert.DeserializeObject<MachineStatus>(stringResponse);
        }

        private List<MachineListItem> DeserializeVmList(dynamic? stringResponse)
        {
            return JsonConvert.DeserializeObject<List<MachineListItem>>(stringResponse);
        }

        #endregion
    }
}