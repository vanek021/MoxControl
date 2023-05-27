using System.ComponentModel.DataAnnotations;

namespace MoxControl.Connect.Models.Enums
{
    public enum MachineStage
    {
        [Display(Name = "Готова к созданию")]
        ReadyForCreate = 0,
        [Display(Name = "Используется")]
        Using
    }
}
