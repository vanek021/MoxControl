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

        public HomeService(IConnectServiceFactory connectServiceFactory, TemplateManager templateManager, ImageManager imageManager)
        {
            _connectServiceFactory = connectServiceFactory;
            _templateManager = templateManager;
            _imageManager = imageManager;
        }

        public async Task<HomeIndexViewModel> GetHomeIndexViewModelAsync()
        {
            var summaryTemplate = await GetTemplateSummaryCardViewModelAsync();
            var summaryImage = await GetImageSummaryCardViewModelAsync();
            var summaryServers = await GetServerSummaryCardViewModelAsync();

            return new HomeIndexViewModel(summaryServers, summaryImage, summaryTemplate, new());
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
