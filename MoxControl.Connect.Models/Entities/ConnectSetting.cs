using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Entities
{
    public class ConnectSetting : BaseRecord
    {
        /// <summary>
        /// Настраиваемая система виртуализации
        /// </summary>
        public VirtualizationSystem VirtualizationSystem { get; set; }

        /// <summary>
        /// Флаг, который означает, имеет ли система виртуализации дополнительные настройки
        /// </summary>
        public bool IsShowSettingsSection { get; set; }
    }
}
