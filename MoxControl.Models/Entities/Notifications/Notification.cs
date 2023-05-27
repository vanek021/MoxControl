using MoxControl.Core.Models;
using MoxControl.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoxControl.Models.Entities.Notifications
{
    public class Notification : BaseRecord
    {
        [ForeignKey(nameof(NotificationReceiverId))]
        public NotificationReceiver NotificationReceiver { get; set; }
        public long NotificationReceiverId { get; set; }

        public NotificationType NotificationType { get; set; } = NotificationType.Common;
        public string Title { get; set; }
        public string Description { get; set; }
        public bool WasViewed { get; set; }
    }
}
