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
        [Display(Name = "Информация")]
        Info,
        [Display(Name = "Успешно")]
        Success,
        [Display(Name = "Ошибка")]
        Error,
        [Display(Name = "Предупреждение")]
        Warning,
        [Display(Name = "Внутренняя ошибка сервера")]
        InternalServerError,
    }
}
