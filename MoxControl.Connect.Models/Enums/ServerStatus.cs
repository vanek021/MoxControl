using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Enums
{
    public enum ServerStatus
    {
        [Display(Name = "Неизвестно")]
        Unknown = 0,
        [Display(Name = "Запущен")]
        Running,
        [Display(Name = "Остановлен")]
        Stopped,
        [Display(Name = "Не стабилен")]
        Unstable
    }
}
