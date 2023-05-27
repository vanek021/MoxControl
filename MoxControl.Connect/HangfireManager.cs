using Hangfire;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Services;

namespace MoxControl.Connect
{
    public class HangfireConnectManager
    {
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly ImageManager _imageManager;
        private readonly TemplateManager _templateManager;

        public HangfireConnectManager(IConnectServiceFactory connectServiceFactory, ImageManager imageManager, TemplateManager templateManager)
        {
            _connectServiceFactory = connectServiceFactory;
            _imageManager = imageManager;
            _templateManager = templateManager;
        }

        public async Task HangfireSendServerHeartBeat(VirtualizationSystem virtualizationSystem, long serverId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Servers.SendHeartBeatAsync(serverId, initiatorUsername);
        }

        public async Task HangfireSendHeartBeatToAllServers()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();
                servers.ForEach(s => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.HangfireSendServerHeartBeat(connectServiceItem.VirtualizationSystem, s.Id, null)));
            }
        }

        public async Task HangfireSendMachineHeartBeat(VirtualizationSystem virtualizationSystem, long machineId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Machines.SendHeartBeatAsync(machineId, initiatorUsername);
        }

        public async Task HangfireSendHeartBeatToAllMachines()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var machines = await connectServiceItem.Service.Machines.GetAllAsync();
                machines.ForEach(m => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.HangfireSendMachineHeartBeat(connectServiceItem.VirtualizationSystem, m.Id, null)));
            }
        }

        public async Task HangfireSyncMachinesForAllServers()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();
                servers.ForEach(s => BackgroundJob.Enqueue<HangfireConnectManager>(h => HangfireSyncServerMachines(connectServiceItem.VirtualizationSystem, s.Id, null)));
            }
        }

        public async Task HangfireSyncServerMachines(VirtualizationSystem virtualizationSystem, long serverId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Servers.SyncMachinesAsync(serverId, initiatorUsername);
        }

        public async Task HangifreDeliverImageToAllServers(long imageId, string? initiatorUsername = null)
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();
                servers.ForEach(s => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.HangfireDeliverImageToServer(connectServiceItem.VirtualizationSystem, s.Id, imageId, initiatorUsername)));
            }

            await _imageManager.UpdateStatusAsync(imageId, ISOImageStatus.ReadyToUse);
        }

        public async Task HangfireDeliverImageToServer(VirtualizationSystem virtualizationSystem, long serverId, long imageId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Servers.UploadImageAsync(serverId, imageId, initiatorUsername);
        }

        public async Task HangfireHandleTemplateCreateForAllServers(long templateId, string? initiatorUsername = null)
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                await connectServiceItem.Service.Servers.HandleCreateTemplateAsync(templateId, initiatorUsername);
            }

            await _templateManager.MarkAsReadyToUseAsync(templateId);
        }

        public async Task HangfireProccessCreateMachineAsync(VirtualizationSystem virtualizationSystem, long machineId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            await connectService.Machines.ProcessCreateAsync(machineId, initiatorUsername);
        }

        public static void RegisterJobs()
        {
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.HangfireSendHeartBeatToAllServers(), Cron.Hourly());
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.HangfireSendHeartBeatToAllMachines(), Cron.Hourly());
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.HangfireSyncMachinesForAllServers(), Cron.Hourly());
        }
    }
}
