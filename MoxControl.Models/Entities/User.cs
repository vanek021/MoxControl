using MoxControl.Core.Models;
using MoxControl.Models.Entities.Notifications;

namespace MoxControl.Models.Entities
{
    public class User : BaseUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public NotificationReceiver NotificationReceiver { get; set; } = new();
        public string? TelegramName { get; set; }
        public bool TelegramNotifyEnabled { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
