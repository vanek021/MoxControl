using Hangfire;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Connect;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Services;
using MoxControl.Infrastructure.Extensions;
using MoxControl.ViewModels.SyncViewModels;

namespace MoxControl.Services
{
    public class SyncService
    {
        private readonly ImageManager _imageManager;
        private readonly TemplateManager _templateManager;
        private readonly HangfireConnectManager _hangfireConnectManager;
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SyncService(TemplateManager templateManager, ImageManager imageManager, 
            HangfireConnectManager hangfireConnectManager, IConnectServiceFactory connectServiceFactory, IHttpContextAccessor httpContextAccessor)
        {
            _templateManager = templateManager;
            _imageManager = imageManager;
            _hangfireConnectManager = hangfireConnectManager;
            _connectServiceFactory = connectServiceFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SyncViewModel> GetSyncViewModelAsync()
        {
            var syncViewModel = new SyncViewModel();

            syncViewModel.SyncTemplate.TotalCount = await _templateManager.GetTotalCount();
            syncViewModel.SyncTemplate.InitializedCount = await _templateManager.GetInitializedCount();

            syncViewModel.SyncImage.TotalCount = await _imageManager.GetCountAsync();
            syncViewModel.SyncImage.InitializedCount = await _imageManager.GetInitializedCountAsync();

            var connectServiceItems = _connectServiceFactory.GetAll();

            foreach (var connectService in connectServiceItems)
            {
                var servers = await connectService.Service.Servers.GetAllAsync();
                foreach (var server in servers)
                {
                    var leftImages = syncViewModel.SyncImage.TotalCount - server.ImageData?.ImageIds.Count;
                    if (leftImages > 0)
                        syncViewModel.SyncImage.NotInitializedServersCount += 1;

                    var leftTemplates = syncViewModel.SyncTemplate.TotalCount - server.TemplateData?.TemplateIds.Count;
                    if (leftTemplates > 0)
                        syncViewModel.SyncTemplate.NotInitializedServersCount += 1;

                    syncViewModel.SyncServers.Add(new SyncServerViewModel()
                    {
                        Id = server.Id,
                        Name = server.Name,
                        Description = server.Description,
                        LastMachinesSync = server.LastMachinesSync,
                        VirtualizationSystem = server.VirtualizationSystem
                    });
                }
            }
            return syncViewModel;
        }

        public void StartMachinesSync()
        {
            BackgroundJob.Enqueue<HangfireConnectManager>(h => h.SyncMachinesForAllServers());
        }

        public void StartMachineSync(VirtualizationSystem virtualizationSystem, long serverId)
        {
            BackgroundJob.Enqueue<HangfireConnectManager>(h => h.SyncServerMachinesAsync(virtualizationSystem, serverId, _httpContextAccessor.HttpContext.GetUsername()));
        }

        public void StartTemplatesSync()
        {
            BackgroundJob.Enqueue<HangfireConnectManager>(h => h.SyncTemplatesForAllServersAsync());
        }

        public void StartImagesSync()
        {
            BackgroundJob.Enqueue<HangfireConnectManager>(h => h.SyncImagesForAllServersAsync());
        }
    }
}
