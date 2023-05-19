using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
