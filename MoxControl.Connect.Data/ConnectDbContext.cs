using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MoxControl.Connect.Data
{
    public class ConnectDbContext : DbContext
    {
        public ConnectDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}