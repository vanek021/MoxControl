using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Proxmox.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Services.InternalServices
{
    public class MachineService : IMachineService
    {
        private readonly ConnectProxmoxDbContext _context;

        public MachineService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<ConnectProxmoxDbContext>();
        }

        public async Task<List<BaseMachine>> GetAllByServer(long id)
        {
            var machines = await _context.ProxmoxMachines.Where(x => x.ServerId == id).Select(x => (BaseMachine)x).ToListAsync();

            return machines;
        }
    }
}
