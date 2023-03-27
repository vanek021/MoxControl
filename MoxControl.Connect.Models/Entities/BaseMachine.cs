using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Entities
{
    public abstract class BaseMachine : BaseRecord
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
        /// Оперативная память, в Мб
        /// </summary>
        public int RAMSize { get; set; }

        /// <summary>
        /// Размер жесткого диска, в Гб
        /// </summary>
        public int HDDSize { get; set; }

        public MachineStatus Status { get; set; }
    }
}
