using System.ComponentModel.DataAnnotations;

namespace MoxControl.Models.Enums
{
    public enum GeneralNotificationType
    {
        [Display(Name = "INFO")]
        Info,
        [Display(Name = "OK")]
        Success,
        [Display(Name = "ERR")]
        Error,
        [Display(Name = "WARN")]
        Warning,
        [Display(Name = "ERR 500")]
        InternalServerError,
    }
}
