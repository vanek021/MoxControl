using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Models.Entities;

namespace MoxControl.Connect.Data
{
    public class ConnectDbContext : DbContext
    {
        public ConnectDbContext(DbContextOptions<ConnectDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("connect");
            base.OnModelCreating(builder);
        }

        public DbSet<ConnectSetting> ConnectSettings { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<ISOImage> ISOImages { get; set; }
    }
}