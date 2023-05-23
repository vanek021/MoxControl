using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Models.Entities
{
    public class ProxmoxServer : BaseServer
    {
        public ProxmoxServer()
        {
            VirtualizationSystem = VirtualizationSystem.Proxmox;
        }

        /// <summary>
        /// Базовый узел сервера
        /// </summary>
        public string BaseNode { get; set; } = "pve";

        /// <summary>
        /// Базовое хранилище для образов
        /// </summary>
        public string BaseStorage { get; set; } = "local";

        /// <summary>
        /// Базовое хранилище для дисков виртуальных машин
        /// </summary>
        public string BaseDisksStorage { get; set; } = "local";

        /// <summary>
        /// Тип авторизации
        /// </summary>
        public string Realm { get; set; } = "pam";

        public new virtual List<ProxmoxMachine> Machines { get; set; } = new();
    }
}
