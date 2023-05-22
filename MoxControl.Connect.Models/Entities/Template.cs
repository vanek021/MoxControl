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
    public class Template : BaseRecord
    {
        /// <summary>
        /// Название шаблона
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание шаблона
        /// </summary>
        public string Description { get; set; }

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
        /// Используемый образ
        /// </summary>
        [ForeignKey(nameof(ISOImageId))]
        public ISOImage ISOImage { get; set; }
        public long ISOImageId { get; set; }

        /// <summary>
        /// Сервера, на которых доступен для использования данный шаблон
        /// </summary>
        [Column(TypeName = "jsonb")]
        public TemplateAvailableServerData? AvailableServerData { get; set; }
    }

    public class TemplateAvailableServerData
    {
        public List<TemplateAvailableServerDataItem> Items { get; set; } = new();
    }

    public class TemplateAvailableServerDataItem
    {
        public long ServerId { get; set; }
        public VirtualizationSystem VirtualizationSystem { get; set; }
    }
}
