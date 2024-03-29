﻿using Hangfire;
using MoxControl.Connect.Data;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Services;
using MoxControl.Infrastructure.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace MoxControl.Connect
{
    public class HangfireConnectManager
    {
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly ImageManager _imageManager;
        private readonly TemplateManager _templateManager;
        private readonly ConnectDatabase _connectDb;

        public HangfireConnectManager(IConnectServiceFactory connectServiceFactory, ImageManager imageManager, TemplateManager templateManager, ConnectDatabase connectDb)
        {
            _connectServiceFactory = connectServiceFactory;
            _imageManager = imageManager;
            _templateManager = templateManager;
            _connectDb = connectDb;
        }

        public async Task SendServerHeartBeatAsync(VirtualizationSystem virtualizationSystem, long serverId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Servers.SendHeartBeatAsync(serverId, initiatorUsername);
        }

        public async Task SendHeartBeatToAllServersAsync()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();
                servers.ForEach(s => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.SendServerHeartBeatAsync(connectServiceItem.VirtualizationSystem, s.Id, null)));

                var setting = await _connectDb.ConnectSettings.GetByVirtualizationSystemAsync(connectServiceItem.VirtualizationSystem);
                if (setting is not null)
                {
                    setting.LastServersCheck = DateTime.UtcNow.ToMoscowTime();
                    _connectDb.ConnectSettings.Update(setting);
                    await _connectDb.SaveChangesAsync();
                }
            }
        }

        public async Task SendMachineHeartBeatAsync(VirtualizationSystem virtualizationSystem, long machineId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Machines.SendHeartBeatAsync(machineId, initiatorUsername);
        }

        public async Task SendHeartBeatToAllMachinesAsync()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var machines = await connectServiceItem.Service.Machines.GetAllAsync();
                machines.ForEach(m => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.SendMachineHeartBeatAsync(connectServiceItem.VirtualizationSystem, m.Id, null)));

                var setting = await _connectDb.ConnectSettings.GetByVirtualizationSystemAsync(connectServiceItem.VirtualizationSystem);
                if (setting is not null)
                {
                    setting.LastMachinesCheck = DateTime.UtcNow.ToMoscowTime();
                    _connectDb.ConnectSettings.Update(setting);
                    await _connectDb.SaveChangesAsync();
                }
            }
        }

        public async Task SyncMachinesForAllServers()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();
                servers.ForEach(s => BackgroundJob.Enqueue<HangfireConnectManager>(h => SyncServerMachinesAsync(connectServiceItem.VirtualizationSystem, s.Id, null)));
            }
        }

        public async Task SyncServerMachinesAsync(VirtualizationSystem virtualizationSystem, long serverId, string? initiatorUsername = null)
        {
            var setting = await _connectDb.ConnectSettings.GetByVirtualizationSystemAsync(virtualizationSystem);

            if (setting.IsMachinesSyncEnabled)
            {
                var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
                await connectService.Servers.SyncMachinesAsync(serverId, initiatorUsername);
            }
        }

        public async Task DeliverImageToAllServersAsync(long imageId, string? initiatorUsername = null)
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();
                servers.ForEach(s => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.DeliverImageToServerAsync(connectServiceItem.VirtualizationSystem, s.Id, imageId, initiatorUsername)));
            }

            await _imageManager.UpdateStatusAsync(imageId, ISOImageStatus.ReadyToUse);
        }

        public async Task DeliverImageToServerAsync(VirtualizationSystem virtualizationSystem, long serverId, long imageId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Servers.UploadImageAsync(serverId, imageId, initiatorUsername);
        }

        public async Task HandleTemplateCreateForAllServersAsync(long templateId, string? initiatorUsername = null)
        {
            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectServiceItem in connectServiceItems)
            {
                await connectServiceItem.Service.Servers.HandleCreateTemplateAsync(templateId, initiatorUsername);
            }

            await _templateManager.MarkAsReadyToUseAsync(templateId);
        }

        public async Task ProccessCreateMachineAsync(VirtualizationSystem virtualizationSystem, long machineId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            await connectService.Machines.ProcessCreateAsync(machineId, initiatorUsername);
        }

        public async Task SyncImagesForAllServersAsync()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();
            var images = await _imageManager.GetAllAsync();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();

                foreach (var server in servers)
                {
                    var imageIdsForDelivery = images.Select(i => i.Id).ToList();

                    if (server.ImageData is not null)
                        imageIdsForDelivery = imageIdsForDelivery.Except(server.ImageData.ImageIds).ToList();

                    imageIdsForDelivery.ForEach(i => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.DeliverImageToServerAsync(connectServiceItem.VirtualizationSystem, server.Id, i, null)));
                }
            }
        }

        public async Task SyncTemplatesForAllServersAsync()
        {
            var connectServiceItems = _connectServiceFactory.GetAll();
            var templates = await _templateManager.GetAllAsync();

            foreach (var connectServiceItem in connectServiceItems)
            {
                var servers = await connectServiceItem.Service.Servers.GetAllAsync();

                foreach (var server in servers)
                {
                    var templateIdsForDelivery = templates.Select(i => i.Id).ToList();

                    if (server.TemplateData is not null)
                        templateIdsForDelivery = templateIdsForDelivery.Except(server.TemplateData.TemplateIds).ToList();

                    templateIdsForDelivery.ForEach(t => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.HandleTemplateCreateForServerAsync(server.VirtualizationSystem, server.Id, t)));
                }
            }
        }

        public async Task HandleTemplateCreateForServerAsync(VirtualizationSystem virtualizationSystem, long serverId, long templateId)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);

            await connectService.Servers.HandleCreateTemplateAsync(templateId, null);
        }

        public static void RegisterJobs()
        {
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.SendHeartBeatToAllServersAsync(), "*/5 * * * *");
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.SendHeartBeatToAllMachinesAsync(), "*/5 * * * *");
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.SyncMachinesForAllServers(), Cron.Hourly());
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.SyncImagesForAllServersAsync(), Cron.Daily());
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.SyncTemplatesForAllServersAsync(), Cron.Daily());
        }
    }
}
