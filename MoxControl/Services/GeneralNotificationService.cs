using MoxControl.Data;
using MoxControl.Models.Entities.Notifications;
using MoxControl.Services.Abtractions;

namespace MoxControl.Services
{
    public class GeneralNotificationService : ServiceBase<GeneralNotification>
    {
        public GeneralNotificationService(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
