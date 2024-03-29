﻿using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Proxmox.Models.Entities;

namespace MoxControl.Connect.Proxmox.Data
{
    public class ConnectProxmoxDbContext : DbContext
    {
        public ConnectProxmoxDbContext(DbContextOptions<ConnectProxmoxDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("connect_proxmox");
            base.OnModelCreating(builder);
        }

        public DbSet<ProxmoxServer> ProxmoxServers { get; set; }
        public DbSet<ProxmoxMachine> ProxmoxMachines { get; set; }
        public DbSet<TemplateMachine> TemplateMachines { get; set; }
    }
}