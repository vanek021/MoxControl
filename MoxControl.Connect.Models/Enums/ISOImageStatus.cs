using System.ComponentModel.DataAnnotations;

namespace MoxControl.Connect.Models.Enums
{
    public enum ISOImageStatus
    {
        [Display(Name = "Инициализируется")]
        Initializing = 0,
        [Display(Name = "Доставляется")]
        Delivering,
        [Display(Name = "Готов к работе")]
        ReadyToUse
    }
}
