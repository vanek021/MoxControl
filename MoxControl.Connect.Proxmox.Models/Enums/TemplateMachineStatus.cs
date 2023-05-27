using System.ComponentModel.DataAnnotations;

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
