using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.SyncViewModels
{
    public class SyncServerViewModel
    {
        public long Id { get; set; }
        public VirtualizationSystem VirtualizationSystem { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? LastMachinesSync { get; set; }
    }
}
