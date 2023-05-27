using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;

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
