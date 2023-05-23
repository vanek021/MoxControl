using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Models.Result;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;
using MoxControl.Connect.Proxmox.VirtualizationClient.Helpers;
using MoxControl.Connect.Services.InternalServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MoxControl.Connect.Proxmox.Services.InternalServices
{
    public class MachineService : BaseMachineService, IMachineService
    {
        private readonly ConnectProxmoxDbContext _context;

        public MachineService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
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

        public async Task<List<BaseMachine>> GetAllAsync()
            => await _context.ProxmoxMachines.Where(x => !x.IsDeleted).Select(x => (BaseMachine)x).ToListAsync();

        private async Task<ProxmoxMachine?> GetWithServerAsync(long id)
            => await _context.ProxmoxMachines.Where(x => !x.IsDeleted).Include(x => x.Server).FirstOrDefaultAsync(m => m.Id == id);

        #endregion

        public async Task<BaseMachine?> GetAsync(long id)
        {
            var machine = await _context.ProxmoxMachines
                .Where(x => !x.IsDeleted)
                .Include(x => x.Server)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (machine is null)
                return null;

            var baseMachine = (BaseMachine)machine;
            baseMachine.Server = machine.Server;

            return baseMachine;
        }

        public async Task<string?> GetConsoleSourceAsync(long id)
        {
            var machine = await _context.ProxmoxMachines
                .Where(x => !x.IsDeleted)
                .Include(x => x.Server)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (machine is null)
                return null;

            return $"https://{machine.Server.Host}:{machine.Server.Port}" +
                $"/?console=kvm&novnc=1&vmid={machine.ProxmoxId}&vmname={machine.ProxmoxName}" +
                $"&node={machine.Server.BaseNode}&resize=off&cmd=";
        }

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

        public async Task<MachineHealthModel?> GetHealthModel(long machineId, string? initiatorUsername = null)
        {
            var machine = await _context.ProxmoxMachines.Include(m => m.Server).FirstOrDefaultAsync(m => m.Id == machineId);

            if (machine is null || !machine.ProxmoxId.HasValue)
                return null;

            var credentials = GetServerCredentials(machine.Server, initiatorUsername);

            var client = new ProxmoxVirtualizationClient(machine.Server.Host, machine.Server.Port, credentials.Login, credentials.Password);

            var machineStatus = await client.GetMachineStatus(machine.ProxmoxId.Value);
            var status = machineStatus.Status.GetMachineStatus();

            if (status != MachineStatus.Running)
                return null;

            var rrddataItems = await client.GetMachineRrddata(machine.ProxmoxId.Value);
            var lastData = rrddataItems.LastOrDefault();

            if (lastData is null)
                return null;

            var machineHealthModel = new MachineHealthModel(long.Parse(lastData.MemoryTotal!),
                double.Parse(lastData.MemoryUsed!, CultureInfo.InvariantCulture), long.Parse(lastData.HDDTotal!),
                double.Parse(lastData.HDDUsed!, CultureInfo.InvariantCulture), double.Parse(lastData.CPUUsed!, CultureInfo.InvariantCulture));

            return machineHealthModel;
        }

        public async Task SendHeartBeat(long machineId, string? initiatorUsername = null)
        {
            var machine = await _context.ProxmoxMachines.Include(m => m.Server).FirstOrDefaultAsync(x => x.Id == machineId && !x.IsDeleted);

            if (machine is null || !machine.ProxmoxId.HasValue)
                return;

            var credentials = GetServerCredentials(machine.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(machine.Server.Host, machine.Server.Port, credentials.Login, credentials.Password);

                var status = await proxmoxVirtualizationSystem.GetMachineStatus(machine.ProxmoxId.Value);

                await SendTelegramMachineNotify(machine.Server.Name, machine.Name, machine.Status, status.Status.GetMachineStatus());
                machine.Status = status.Status.GetMachineStatus();
            }
            catch
            {
                await SendTelegramMachineNotify(machine.Server.Name, machine.Name, machine.Status, MachineStatus.Unknown);
                machine.Status = MachineStatus.Unknown;
            }
            finally
            {
                _context.Update(machine);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BaseResult> TurnOff(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(machine.Server.Host, machine.Server.Port, credentials.Login, credentials.Password);

                var result = await proxmoxVirtualizationSystem.ShutdownMachine(machine!.ProxmoxId!.Value);

                if (result.Success)
                {
                    machine.Status = MachineStatus.Stopped;
                    _context.ProxmoxMachines.Update(machine);
                    await _context.SaveChangesAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new(false, ex.ToString());
            }
        }

        public async Task<BaseResult> TurnOn(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(machine.Server.Host, machine.Server.Port, credentials.Login, credentials.Password);

                var result = await proxmoxVirtualizationSystem.StartMachine(machine!.ProxmoxId!.Value);

                if (result.Success)
                {
                    machine.Status = MachineStatus.Running;
                    _context.ProxmoxMachines.Update(machine);
                    await _context.SaveChangesAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new(false, ex.ToString());
            }
        }

        public async Task<BaseResult> Reboot(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(machine.Server.Host, machine.Server.Port, credentials.Login, credentials.Password);

                var result = await proxmoxVirtualizationSystem.RebootMachine(machine!.ProxmoxId!.Value);

                return result;
            }
            catch (Exception ex)
            {
                return new(false, ex.ToString());
            }
        }

        public async Task<BaseResult> HardReboot(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(machine.Server.Host, machine.Server.Port, credentials.Login, credentials.Password);

                var result = await proxmoxVirtualizationSystem.ResetMachine(machine!.ProxmoxId!.Value);

                return result;
            }
            catch (Exception ex)
            {
                return new(false, ex.ToString());
            }
        }

        private BaseResult? ValidateMachineBeforeAction(ProxmoxMachine? machine)
        {
            if (machine is null)
                return new(false, "Виртуальная машина не найдена в базе данных сервиса");

            if (machine.ProxmoxId is null)
                return new(false, "Идентификатор машины в Proxmox не установлен");

            return null;
        }
    }
}
