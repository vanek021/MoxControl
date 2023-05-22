using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
