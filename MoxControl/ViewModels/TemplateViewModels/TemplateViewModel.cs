using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.TemplateViewModels
{
    public class TemplateViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TemplateStatus Status { get; set; }
    }
}
