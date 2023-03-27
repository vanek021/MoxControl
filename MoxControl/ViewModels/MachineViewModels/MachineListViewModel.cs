using MoxControl.Connect.Models.Enums;
using MoxControl.ViewModels.ServerViewModels;

namespace MoxControl.ViewModels.MachineViewModels
{
    public class MachineListViewModel
    {
        public ServerViewModel Server { get; set; }
        public List<MachineViewModel> Machines { get; set; }
    }
}
