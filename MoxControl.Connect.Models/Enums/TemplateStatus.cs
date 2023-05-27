using System.ComponentModel.DataAnnotations;

namespace MoxControl.Connect.Models.Enums
{
    public enum TemplateStatus
    {
        [Display(Name = "Инициализируется")]
        Initializing,
        [Display(Name = "Готов к работе")]
        ReadyToUse
    }
}
