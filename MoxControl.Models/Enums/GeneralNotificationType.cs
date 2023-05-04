using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
