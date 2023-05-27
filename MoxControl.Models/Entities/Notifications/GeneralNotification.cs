using MoxControl.Core.Models;
using MoxControl.Models.Enums;

namespace MoxControl.Models.Entities.Notifications
{
    public class GeneralNotification : BaseRecord
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public GeneralNotificationType Type { get; set; }
    }
}
