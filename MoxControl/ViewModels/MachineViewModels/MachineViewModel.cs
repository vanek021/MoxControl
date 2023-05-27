using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.MachineViewModels
{
    public class MachineViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MachineStatus Status { get; set; }
    }
}
