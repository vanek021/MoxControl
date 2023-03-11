using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Models.Entities.Notifications
{
    public class NotificationReceiver : BaseRecord
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
        public bool IsEnabled { get; set; } = true;
        public virtual List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
