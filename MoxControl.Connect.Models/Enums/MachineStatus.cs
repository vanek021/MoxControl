using System.ComponentModel.DataAnnotations;

namespace MoxControl.Connect.Models.Enums
{
    public enum MachineStatus
    {
        [Display(Name = "Неизвестно")]
        Unknown = 0,
        [Display(Name = "Запущена")]
        Running,
        [Display(Name = "Остановлена")]
        Stopped
    }
}
