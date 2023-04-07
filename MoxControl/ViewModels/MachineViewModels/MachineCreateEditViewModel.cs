using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MoxControl.ViewModels.MachineViewModels
{
    public class MachineCreateEditViewModel
    {
        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Образ")]
        [Required]
        public long TemplateId { get; set; }

        [ValidateNever]
        public SelectList Templates { get; set; }
    }
}
