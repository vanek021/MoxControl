using System.ComponentModel.DataAnnotations;

namespace MoxControl.Models.Enums
{
    public enum NotificationType
    {
        [Display(Name = "Общий")]
        Common = 0,
        [Display(Name = "Ошибка")]
        Error
    }
}
