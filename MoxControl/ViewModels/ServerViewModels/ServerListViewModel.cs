using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerListViewModel
    {
        public VirtualizationSystem VirtualizationSystem { get; set; }
        public List<ServerViewModel> Servers { get; set; }
    }
}
