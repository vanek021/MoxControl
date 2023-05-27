using MoxControl.Connect.Models.Enums;

namespace MoxControl.ViewModels.TemplateViewModels
{
#pragma warning disable CS8618
    public class TemplateViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TemplateStatus Status { get; set; }
    }
#pragma warning restore CS8618
}
