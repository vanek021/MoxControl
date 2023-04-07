using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        #region lambdas

        public async Task<int> GetTotalCountAsync()
            => await _context.ProxmoxMachines.Where(m => !m.IsDeleted).CountAsync();

        public async Task<List<BaseMachine>> GetAllByServer(long id)
            => await _context.ProxmoxMachines.Where(x => x.ServerId == id && !x.IsDeleted).Select(x => (BaseMachine)x).ToListAsync();

        public async Task<List<BaseMachine>> GetAllWithTemplate()
            => await _context.ProxmoxMachines.Where(x => x.TemplateId.HasValue && !x.IsDeleted).Select(x => (BaseMachine)x).ToListAsync();

        public async Task<int> GetAliveCountAsync()
            => await _context.ProxmoxMachines.Where(x => x.Status == MachineStatus.Running && !x.IsDeleted).CountAsync();

        public async Task<BaseMachine?> GetAsync(long id)
            => await _context.ProxmoxMachines.Where(x => !x.IsDeleted).Select(x => (BaseMachine)x).FirstOrDefaultAsync(m => m.Id == id);

        #endregion

        public async Task<bool> CreateAsync(BaseMachine machine, long serverId, long? templateId = null)
        {
            var proxmoxMachine = new ProxmoxMachine()
            {
                Name = machine.Name,
                Description = machine.Description,
                ServerId = serverId,
                TemplateId = templateId,
                RAMSize = machine.RAMSize,
                HDDSize = machine.HDDSize,
                CPUCores = machine.CPUCores,
                CPUSockets = machine.CPUSockets,
                Status = machine.Status,
                Stage = machine.Stage,
            };

            _context.ProxmoxMachines.Add(proxmoxMachine);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(BaseMachine machineModel)
        {
            var machine = await _context.ProxmoxMachines.FirstOrDefaultAsync(m => m.Id == machineModel.Id && !m.IsDeleted);

            if (machine is null)
                return false;

            machine.Name = machineModel.Name;
            machine.Description = machineModel.Description;
            machine.RAMSize = machineModel.RAMSize;
            machine.HDDSize = machineModel.HDDSize;
            machine.CPUCores = machineModel.CPUCores;
            machine.CPUSockets = machineModel.CPUSockets;
            machine.Status = machineModel.Status;
            machine.Stage = machineModel.Stage;

            _context.ProxmoxMachines.Update(machine);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var machine = await _context.ProxmoxMachines.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (machine is null)
                return false;

            machine.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
