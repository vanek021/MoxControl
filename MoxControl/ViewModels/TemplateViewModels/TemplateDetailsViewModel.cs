using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.TemplateViewModels
{
#pragma warning disable CS8618
    public class TemplateDetailsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RAMSize { get; set; }
        public int HDDSize { get; set; }
        public int CPUSockets { get; set; }
        public int CPUCores { get; set; }
        public TemplateStatus Status { get; set; }
        public List<TemplateServerViewModel> Servers { get; set; } = new();
    }
#pragma warning restore CS8618
}
