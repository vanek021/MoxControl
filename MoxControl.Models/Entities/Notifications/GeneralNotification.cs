using MoxControl.Core.Models;
using MoxControl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Models.Entities.Notifications
{
    public class GeneralNotification : BaseRecord
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public GeneralNotificationType Type { get; set; }
    }
}
