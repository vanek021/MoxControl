using MoxControl.Connect.Data;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Services;
using MoxControl.ViewModels.HomeViewModels;

namespace MoxControl.Services
{
    public class HomeService
    {
        private readonly IConnectServiceFactory _connectServiceFactory;
        private readonly ImageManager _imageManager;
        private readonly TemplateManager _templateManager;
        private readonly ConnectDatabase _connectDatabase;

        public HomeService(IConnectServiceFactory connectServiceFactory, TemplateManager templateManager, ImageManager imageManager, ConnectDatabase connectDatabase)
        {
            _connectServiceFactory = connectServiceFactory;
            _templateManager = templateManager;
            _imageManager = imageManager;
            _connectDatabase = connectDatabase;
        }

        public async Task<HomeIndexViewModel> GetHomeIndexViewModelAsync()
        {
            var summaryTemplate = await GetTemplateSummaryCardViewModelAsync();
            var summaryImage = await GetImageSummaryCardViewModelAsync();
            var summaryServers = await GetServerSummaryCardViewModelAsync();
            var systemSummaries = await GetSystemSummaryViewModelsAsync();

            return new HomeIndexViewModel(summaryServers, summaryImage, summaryTemplate, systemSummaries);
        }

        private async Task<List<SystemSummaryViewModel>> GetSystemSummaryViewModelsAsync()
        {
            var connectServices = _connectServiceFactory.GetAll();
            var result = new List<SystemSummaryViewModel>();

            foreach (var connectService in connectServices)
            {
                var totalServers = await connectService.Item2.Servers.GetTotalCountAsync();
                var totalMachines = await connectService.Item2.Machines.GetTotalCountAsync();
                var aliveServers = await connectService.Item2.Servers.GetAliveCountAsync();
                var aliveMachines = await connectService.Item2.Machines.GetAliveCountAsync();
                var connectSetting = await _connectDatabase.ConnectSettings.GetByVirtualizationSystemAsync(connectService.Item1);
                var lastServersCheck = connectSetting.LastServersCheck;

                result.Add(new SystemSummaryViewModel(connectService.Item1, totalServers, totalMachines, aliveServers, aliveMachines, lastServersCheck));
            }

            return result;
        }

        private async Task<SummaryCardViewModel> GetTemplateSummaryCardViewModelAsync()
        {
            var templates = await _templateManager.GetAllAsync();
            var usedTemplatesIds = new List<long>();
            var connectServices = _connectServiceFactory.GetAll();

            foreach (var connectService in connectServices)
            {
                var machinesWithTemplate = await connectService.Item2.Machines.GetAllWithTemplate();
                machinesWithTemplate.ForEach(x => usedTemplatesIds.Add(x.TemplateId!.Value));
            }

            var totalCount = templates.Count;
            var usedCount = usedTemplatesIds.Distinct().Count();
            var frequentlyUsed = usedTemplatesIds.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
            var mostPopular = await _templateManager.GetByIdAsync(frequentlyUsed);

            return new SummaryCardViewModel(templates.Count, usedTemplatesIds.Distinct().Count(), mostPopular?.Name);
        }

        private async Task<SummaryCardViewModel> GetImageSummaryCardViewModelAsync()
        {
            var images = await _imageManager.GetAllAsync();
            var templates = await _templateManager.GetAllAsync();
            var usedImages = new List<long>();

            templates.ForEach(x => usedImages.Add(x.ISOImageId));

            var totalCount = images.Count;
            var usedCount = usedImages.Distinct().Count();
            var frequentlyUsed = usedImages.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
            var mostPopular = await _imageManager.GetByIdAsync(frequentlyUsed);

            return new SummaryCardViewModel(totalCount, usedCount, mostPopular?.Name);
        }

        private async Task<SummaryCardViewModel> GetServerSummaryCardViewModelAsync()
        {
            var serversBySystem = new Dictionary<VirtualizationSystem, int>();
            var connectServices = _connectServiceFactory.GetAll();

            foreach (var connectService in connectServices)
            {
                serversBySystem.Add(connectService.Item1, 0);
                var servers = await connectService.Item2.Servers.GetAllAsync();

                servers.ForEach(x => serversBySystem[connectService.Item1]++);
            }

            var totalCount = serversBySystem.Keys.Count();
            var totalServers = serversBySystem.Values.Sum();

            var maxValue = 0;
            string? system = null;

            foreach (var e in serversBySystem)
            {
                if (e.Value > maxValue)
                {
                    maxValue = e.Value;
                    system = e.Key.ToString();
                }
            }

            return new SummaryCardViewModel(totalCount, totalServers, system);
        }
    }
}
