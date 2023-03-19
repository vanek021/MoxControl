using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Entities
{
    public class BaseServer : BaseRecord
    {
        /// <summary>
        /// Используемая система виртуализации
        /// </summary>
        public VirtualizationSystem VirtualizationSystem { get; set; }

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
        public string? RootLogin { get; set; }

        /// <summary>
        /// Пароль, используемый по умолчанию для авторизации
        /// </summary>
        public string? RootPassword { get; set; }

        /// <summary>
        /// Имя сервера, заданное в сервисе
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание сервера, заданное в сервисе
        /// </summary>
        public string Description { get; set; }
    }
}
