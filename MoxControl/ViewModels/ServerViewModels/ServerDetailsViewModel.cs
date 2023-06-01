using MoxControl.ViewModels.MachineViewModels;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerDetailsViewModel : ServerViewModel
    {
        public string? WebUISource { get; set; }
        public List<MachineViewModel> Machines { get; set; } = new();
    }
}
