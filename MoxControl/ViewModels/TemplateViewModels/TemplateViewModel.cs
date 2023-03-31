using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.TemplateViewModels
{
    public class TemplateViewModel
    {
        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }
    }
}
