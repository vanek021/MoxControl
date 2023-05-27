using MoxControl.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

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
