using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.SettingViewModels
{
    public class ConnectSettingViewModel
    {
        [Display(Name = "Показывать секцию настроек сервера")]
        public bool IsShowSettingsSection { get; set; }

        [Display(Name = "Синхронизация с ВМ на сервере")]
        public bool IsMachinesSyncEnabled { get; set; }

        public VirtualizationSystem VirtualizationSystem { get; set; }
    }
}
