using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Data;
using MoxControl.Core.Models;
using MoxControl.Models.Entities;
using MoxControl.Models.Entities.Notifications;
using MoxControl.Models.Entities.Settings;

namespace MoxControl.Data
{
    public class AppDbContext : BaseDbContext<User, BaseRole>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<NotificationReceiver> NotificationReceivers { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<GeneralSetting> GeneralSettings { get; set; }
    }
}
