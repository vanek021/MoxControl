using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
