using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.MachineViewModels
{
    public class MachineDetailsViewModel : MachineViewModel
    {
        public VirtualizationSystem VirtualizationSystem { get; set; }

        public long? ServerId { get; set; }

        public string? ServerName { get; set; }

        public string? ServerHost { get; set; }

        public string? ConsoleHref { get; set; }

        public int? ServerPort { get; set; }

        public int RAMSize { get; set; }

        public int HDDSize { get; set; }

        public int CPUSockets { get; set; }

        public int CPUCores { get; set; }

        public MachineStage Stage { get; set; }
    }
}
