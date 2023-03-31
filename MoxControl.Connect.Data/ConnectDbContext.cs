using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Models.Entities;
using System.Reflection.Emit;

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

        public DbSet<MachineTemplate> MachineTemplates { get; set; }
        public DbSet<ISOImage> ISOImages { get; set; }
    }
}