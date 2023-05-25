using MoxControl.Connect.Interfaces;
using Corsinvest.ProxmoxVE.Api;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using MoxControl.Connect.Proxmox.VirtualizationClient.DTOs;
using MoxControl.Connect.Models.Result;
using MoxControl.Connect.Proxmox.VirtualizationClient.Helpers;
using System.Runtime.CompilerServices;

namespace MoxControl.Connect.Proxmox
{
    public class ProxmoxVirtualizationClient
    {
        private readonly string _baseNode;
        private readonly string _baseStorage;
        private readonly PveClient _pveClient;

        public ProxmoxVirtualizationClient(string host, int port, string login, string password, string realm = "pam", string baseNode = "pve", string baseStorage = "local")
        {
            _pveClient = new PveClient(host, port);
            _pveClient.Login(login, password, realm).GetAwaiter().GetResult();
            _baseNode = baseNode;
            _baseStorage = baseStorage;
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

        public async Task InsertImage(string imageFileName, string imageUrl)
        {
            var result = await _pveClient.Nodes[_baseNode].Storage[_baseStorage].DownloadUrl.DownloadUrl("iso", imageFileName, imageUrl);

            if (result.ResponseInError)
                throw new Exception($"Не удалось загрузить образ {imageFileName} на {_pveClient.Host}:{_pveClient.Port}. {result.StatusCode}");
        }

        public async Task<MachineStatus> CreateTemplateMachine(string name, string image, int cpuSockets, int cpuCores, int ramSize, int hddSize, string disksStorage, string imageStorage)
        {
            var createMachineResult = await CreateMachine(name, image, cpuSockets, cpuCores, ramSize, hddSize, disksStorage, imageStorage);

            await _pveClient.WaitForTaskToFinish(createMachineResult.UniqueTaskId);

            var result = await _pveClient.Nodes[_baseNode].Qemu[createMachineResult.VmId].Template.Template();

            await _pveClient.WaitForTaskToFinish(result.GetUniqueTaskId());

            return await GetMachineStatus(createMachineResult.VmId);
        }

        public async Task<CreateMachineResult> CreateMachine(string name, string image, int cpuSockets, int cpuCores, int ramSize, int hddSize, string disksStorage, string imageStorage)
        {
            var machines = await GetNodeMachines();
            var vmId = machines.Count == 0 ? 100 : machines.Max(m => m.VMid) + 1;

            var ideConfig = new Dictionary<int, string>
            {
                { 2, $"{imageStorage}:iso/{image},media=cdrom" }
            };

            var scsiConfig = new Dictionary<int, string>
            {
                { 0, $"{disksStorage}:{hddSize},iothread=1" }
            };

            var result = await _pveClient.Nodes[_baseNode].Qemu.CreateVm(vmId,
                name: name.Replace(" ", "-"),
                cpuunits: cpuSockets,
                cores: cpuCores,
                memory: ramSize,
                kvm: false,
                ideN: ideConfig,
                scsiN: scsiConfig,
                scsihw: "virtio-scsi-single",
                boot: "order=ide2;scsi0");

            var taskId = result.GetUniqueTaskId();

            return new CreateMachineResult(taskId, vmId);
        }

        public async Task<BaseResult> ShutdownMachine(int machineId)
        {
            var vm = _pveClient.Nodes[_baseNode].Qemu[machineId];

            var result = await vm.Status.Stop.VmStop();

            if (result.InError())
                return new(false, result.GetError());

            var taskId = ((string)JsonConvert
                .SerializeObject(result.Response.data, Formatting.Indented))
                .TrimStart('"').TrimEnd('"');

            await _pveClient.WaitForTaskToFinish(taskId);

            return new(true);
        }

        public async Task<BaseResult> ResetMachine(int machineId)
        {
            var vm = _pveClient.Nodes[_baseNode].Qemu[machineId];

            var result = await vm.Status.Reset.VmReset();

            if (result.InError())
                return new(false, result.GetError());

            var taskId = ((string)JsonConvert
                .SerializeObject(result.Response.data, Formatting.Indented))
                .TrimStart('"').TrimEnd('"');

            await _pveClient.WaitForTaskToFinish(taskId);

            return new(true);

        }

        public async Task<BaseResult> RebootMachine(int machineId)
        {
            var vm = _pveClient.Nodes[_baseNode].Qemu[machineId];

            var result = await vm.Status.Reboot.VmReboot();

            if (result.InError())
                return new(false, result.GetError());

            var taskId = ((string)JsonConvert
                .SerializeObject(result.Response.data, Formatting.Indented))
                .TrimStart('"').TrimEnd('"');

            await _pveClient.WaitForTaskToFinish(taskId);

            return new(true);
        }

        public async Task<BaseResult> StartMachine(int machineId)
        {
            var vm = _pveClient.Nodes[_baseNode].Qemu[machineId];

            var result = await vm.Status.Start.VmStart();

            if (result.InError())
                return new(false, result.GetError());

            var taskId = ((string)JsonConvert
                .SerializeObject(result.Response.data, Formatting.Indented))
                .TrimStart('"').TrimEnd('"');

            await _pveClient.WaitForTaskToFinish(taskId);

            return new(true);
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