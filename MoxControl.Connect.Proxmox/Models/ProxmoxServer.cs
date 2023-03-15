using MoxControl.Connect.Enums;
using MoxControl.Connect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Models
{
    public class ProxmoxServer : BaseServer
    {
        public ProxmoxServer() 
        {
            VirtualizationSystem = VirtualizationSystem.Proxmox;
        }

        /// <summary>
        /// Адрес виртуальной машины
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт виртуальной машины
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Тип авторизации при использовании API клиента
        /// </summary>
        public AuthorizationType AuthorizationType { get; set; } = AuthorizationType.UserCredentials;

        /// <summary>
        /// Логин, используемый по умолчанию для авторизации
        /// </summary>
        public string RootLogin { get; set; }

        /// <summary>
        /// Пароль, используемый по умолчанию для авторизации
        /// </summary>
        public string RootPassword { get; set; }
    }
}
