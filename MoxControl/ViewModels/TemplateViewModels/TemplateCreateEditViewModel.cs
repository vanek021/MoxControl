using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.TemplateViewModels
{
    public class TemplateCreateEditViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Образ")]
        [Required]
        public long ISOImageId { get; set; }

        [Display(Name = "Объем RAM, Мб")]
        [Required]
        public int RAMSize { get; set; }

        [Display(Name = "Объем HDD, Гб")]
        public int HDDSize { get; set; }

        [Display(Name = "Количество сокетов")]
        [Required]
        public int CPUSockets { get; set; }

        [Display(Name = "Количество ядер")]
        [Required]
        public int CPUCores { get; set; }

        [ValidateNever]
        public SelectList Images { get; set; } 
    }
}
