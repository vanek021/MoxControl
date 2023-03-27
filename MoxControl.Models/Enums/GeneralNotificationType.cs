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
        [Display(Name = "Инфо")]
        Info = 0,
        [Display(Name = "Успех")]
        Success,
        [Display(Name = "Ошибка")]
        Error,
        [Display(Name = "Пред")]
        Warning
    }
}
