using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Enums
{
    public enum MachineStage
    {
        [Display(Name = "Готова к созданию")]
        ReadyForCreate = 0,
    }
}
