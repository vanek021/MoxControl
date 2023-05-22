using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Models.Enums
{
    public enum TemplateMachineStatus
    {
        [Display(Name = "Готова к конвертации")]
        ReadyToConvert,
        [Display(Name = "Сконвертирована")]
        Converted
    }
}
