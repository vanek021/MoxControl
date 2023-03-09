using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Data;
using MoxControl.Core.Models;
using MoxControl.Models.Entities;
using MoxControl.Models.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Data
{
    public class AppDbContext : BaseDbContext<User, BaseRole>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<GeneralSetting> GeneralSettings { get; set; }
    }
}
