using MoxControl.Core.Models;
using MoxControl.Models.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Models.Entities
{
    public class User : BaseUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public NotificationReceiver NotificationReceiver { get; set; } = new();
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
