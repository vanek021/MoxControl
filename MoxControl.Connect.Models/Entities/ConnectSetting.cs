using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Models;

namespace MoxControl.Connect.Models.Entities
{
    public class ConnectSetting : BaseRecord
    {
        /// <summary>
        /// Настраиваемая система виртуализации
        /// </summary>
        public VirtualizationSystem VirtualizationSystem { get; set; }

        /// <summary>
        /// Флаг, который означает, имеет ли система виртуализации дополнительные настройки сервера
        /// </summary>
        public bool IsShowSettingsSection { get; set; }

        /// <summary>
        /// Включена ли синхронизация виртуальных машин с теми, которые расположены непосредственно на сервере
        /// </summary>
        public bool IsMachinesSyncEnabled { get; set; }

        /// <summary>
        /// Имеет ли система виртуализации графический интерфейс управления серверами
        /// </summary>
        public bool IsSystemHasInterface { get; set; }

        /// <summary>
        /// Дата последней проверки состояния серверов
        /// </summary>
        public DateTime? LastServersCheck { get; set; }

        /// <summary>
        /// Дата последней проверки состояния виртуальных машин
        /// </summary>
        public DateTime? LastMachinesCheck { get; set; }
    }
}
