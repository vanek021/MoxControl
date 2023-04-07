using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.MachineViewModels
{
    public class MachineViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }

        public MachineStatus Status { get; set; }
    }
}
