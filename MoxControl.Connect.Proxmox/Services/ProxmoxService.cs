using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;
using MoxControl.Infrastructure.Services;
using Sakura.AspNetCore;

namespace MoxControl.Connect.Proxmox.Services
{
    public class ProxmoxService : IConnectService
    {
        public IServerService Servers { get; set; }
        public IMachineService Machines { get; set; }

        public Task Initialize(IServiceScopeFactory serviceScopeFactory)
        {
            Servers = new ServerService(serviceScopeFactory);
            Machines = new MachineService(serviceScopeFactory);
            return Task.CompletedTask;
        }
    }
}
