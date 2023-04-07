using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.HomeViewModels
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel(SummaryCardViewModel serversSummary, SummaryCardViewModel imagesSummary, 
            SummaryCardViewModel templatesSummary, List<SystemSummaryViewModel> systemSummaries)
        {
            ServersSummary = serversSummary;
            ImagesSummary = imagesSummary;
            TemplatesSummary = templatesSummary;
            SystemSummaries = systemSummaries;
        }

        public SummaryCardViewModel ServersSummary { get; set; }

        public SummaryCardViewModel ImagesSummary { get; set; }

        public SummaryCardViewModel TemplatesSummary { get; set; }

        public List<SystemSummaryViewModel> SystemSummaries { get; set; } = new();

    }

    public class SummaryCardViewModel
    {
        public SummaryCardViewModel(int total, int used, string? mostPopular = null)
        {
            Total = total;
            Used = used;
            MostPopular = mostPopular;
        }

        public int Total { get; set; }
        public int Used { get; set; }
        public string? MostPopular { get; set; }
    }

    public class SystemSummaryViewModel
    {
        public SystemSummaryViewModel(VirtualizationSystem virtualizationSystem,  int totalServers, int totalMachines, 
            int serversAlive, int machinesAlive, DateTime? lastServersCheck)
        {
            VirtualizationSystem = virtualizationSystem;
            TotalServers = totalServers;
            TotalMachines = totalMachines;
            LastServersCheck = lastServersCheck;
            ServersAlive = serversAlive;
            MachinesAlive = machinesAlive;
        }

        public int ServersAlive { get; set; }
        public int MachinesAlive { get; set; }
        public VirtualizationSystem VirtualizationSystem { get; set; }
        public int TotalServers { get; set; }
        public int TotalMachines { get; set; }
        public DateTime? LastServersCheck { get; set; }
    }
}
