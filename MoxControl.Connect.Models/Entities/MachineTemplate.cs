using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Entities
{
    public class MachineTemplate : BaseRecord
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
        /// Используемый образ
        /// </summary>
        [ForeignKey(nameof(ISOImageId))]
        public ISOImage ISOImage { get; set; }
        public long ISOImageId { get; set; }
    }
}
