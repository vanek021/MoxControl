using MoxControl.ViewModels.MachineViewModels;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerDetailsViewModel : ServerViewModel
    {
        public List<MachineViewModel> Machines { get; set; } = new();
    }
}
