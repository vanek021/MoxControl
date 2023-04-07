using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MoxControl.ViewModels.MachineViewModels
{
    public class MachineCreateEditViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Шаблон")]
        [Required]
        public long TemplateId { get; set; }

        [ValidateNever]
        public SelectList Templates { get; set; }

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

        public long ServerId { get; set; }

        public VirtualizationSystem VirtualizationSystem { get; set; }
    }
}
