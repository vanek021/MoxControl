using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Entities
{
    public class BaseMachine : BaseRecord, ISoftDeletable
    {
        /// <summary>
        /// Название вирутальной машины
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание виртуальной машины
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Сервер, на котором создается виртуальная машина
        /// </summary>
        [ForeignKey(nameof(ServerId))]
        public BaseServer Server { get; set; }
        public long ServerId { get; set; }

        /// <summary>
        /// Шаблон, использованный при создании виртуальной машины
        /// </summary>
        public long? TemplateId { get; set; }

        /// <summary>
        /// Оперативная память, в Мб
        /// </summary>
        public int RAMSize { get; set; }

        /// <summary>
        /// Размер жесткого диска, в Гб
        /// </summary>
        public int HDDSize { get; set; }

        /// <summary>
        /// Количество сокетов
        /// </summary>
        public int CPUSockets { get; set; }

        /// <summary>
        /// Количество ядер
        /// </summary>
        public int CPUCores { get; set; }

        /// <summary>
        /// Статус виртуальной машины
        /// </summary>
        public MachineStatus Status { get; set; }

        /// <summary>
        /// Этап в момент создания виртуальной машины
        /// </summary>
        public MachineStage Stage { get; set; }

        /// <summary>
        /// Флаг, обозначающий, удалена ли виртуальная машина
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
