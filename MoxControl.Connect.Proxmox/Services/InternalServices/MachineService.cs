using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Models.Result;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;
using MoxControl.Connect.Proxmox.VirtualizationClient.DTOs;
using MoxControl.Connect.Proxmox.VirtualizationClient.Helpers;
using MoxControl.Connect.Services.InternalServices;
using System.Globalization;

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
            => await GetBaseQuery().CountAsync();

        public async Task<List<BaseMachine>> GetAllByServerAsync(long id)
            => await GetBaseQuery().Where(x => x.ServerId == id).Select(x => (BaseMachine)x).ToListAsync();

        public async Task<List<BaseMachine>> GetAllWithTemplateAsync()
            => await GetBaseQuery().Where(x => x.TemplateId.HasValue).Select(x => (BaseMachine)x).ToListAsync();

        public async Task<int> GetAliveCountAsync()
            => await GetBaseQuery().Where(x => x.Status == Connect.Models.Enums.MachineStatus.Running).CountAsync();

        public async Task<List<BaseMachine>> GetAllAsync()
            => await GetBaseQuery().Select(x => (BaseMachine)x).ToListAsync();

        private async Task<ProxmoxMachine?> GetWithServerAsync(long id)
            => await GetBaseQuery().Include(x => x.Server).FirstOrDefaultAsync(m => m.Id == id);

        private IQueryable<ProxmoxMachine> GetBaseQuery()
            => _context.ProxmoxMachines.Where(m => !m.IsDeleted);

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
            var machine = await GetBaseQuery()
                .Include(x => x.Server)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (machine is null)
                return null;

            return $"https://{machine.Server.Host}:{machine.Server.Port}" +
                $"/?console=kvm&novnc=1&vmid={machine.ProxmoxId}&vmname={machine.ProxmoxName}" +
                $"&node={machine.Server.BaseNode}&resize=off&cmd=";
        }

        public async Task<bool> CreateAsync(BaseMachine machine, long serverId, long? templateId = null, long? imageId = null, string? initiatorUsername = null)
        {
            var proxmoxMachine = new ProxmoxMachine()
            {
                Name = machine.Name,
                Description = machine.Description,
                ServerId = serverId,
                RAMSize = machine.RAMSize,
                HDDSize = machine.HDDSize,
                CPUCores = machine.CPUCores,
                CPUSockets = machine.CPUSockets,
                Status = machine.Status,
                Stage = machine.Stage,
            };

            if (templateId.HasValue && templateId.Value != default)
            {
                var template = await _templateManager.GetByIdAsync(templateId.Value);

                if (template is null)
                    return false;

                proxmoxMachine.TemplateId = template.Id;
                proxmoxMachine.RAMSize = template.RAMSize;
                proxmoxMachine.HDDSize = template.HDDSize;
                proxmoxMachine.CPUCores = template.CPUCores;
                proxmoxMachine.CPUSockets = template.CPUSockets;
            }
            else if (imageId.HasValue && imageId.Value != default)
            {
                var image = await _imageManager.GetByIdAsync(imageId.Value);

                if (image is null)
                    return false;

                proxmoxMachine.ImageId = image.Id;
            }

            _context.ProxmoxMachines.Add(proxmoxMachine);

            try
            {
                await _context.SaveChangesAsync();
                BackgroundJob.Enqueue<HangfireConnectManager>(h => h.ProccessCreateMachineAsync(VirtualizationSystem.Proxmox, proxmoxMachine.Id, initiatorUsername));
                return true;
            }
            catch (Exception ex)
            {
                await _generalNotificationService.AddInternalServerErrorAsync(ex);
                return false;
            }
        }

        public async Task ProcessCreateAsync(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            if (machine is null)
                return;

            var credentials = GetServerCredentials(machine.Server, initiatorUsername);
            var client = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, machine.Server);

            var templateId = machine.TemplateId ?? default;
            var imageId = machine.ImageId ?? default;

            CreateMachineResult? createMachineResult;

            if (templateId != default)
            {
                var template = await _templateManager.GetByIdWithImageAsync(templateId);

                if (template is null)
                    return;

                createMachineResult = await client.CreateMachine(machine.Name, Path.GetFileName(template.ISOImage.ImagePath), machine.CPUSockets, machine.CPUCores, machine.RAMSize, machine.HDDSize, machine.Server.BaseDisksStorage, machine.Server.BaseStorage);
            }
            else
            {
                var image = await _imageManager.GetByIdAsync(imageId);

                if (image is null)
                    return;

                createMachineResult = await client.CreateMachine(machine.Name, Path.GetFileName(image.ImagePath), machine.CPUSockets, machine.CPUCores, machine.RAMSize, machine.HDDSize, machine.Server.BaseDisksStorage, machine.Server.BaseStorage);
            }

            if (createMachineResult is not null)
            {
                machine.ProxmoxId = createMachineResult.VmId;
                machine.Stage = MachineStage.Using;
                machine.Status = Connect.Models.Enums.MachineStatus.Stopped;

                _context.ProxmoxMachines.Update(machine);
                await _context.SaveChangesAsync();
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

        public async Task<MachineHealthModel?> GetHealthModelAsync(long machineId, string? initiatorUsername = null)
        {
            var machine = await _context.ProxmoxMachines.Include(m => m.Server).FirstOrDefaultAsync(m => m.Id == machineId);

            if (machine is null || !machine.ProxmoxId.HasValue)
                return null;

            var credentials = GetServerCredentials(machine.Server, initiatorUsername);

            var client = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, machine.Server);

            var machineStatus = await client.GetMachineStatus(machine.ProxmoxId.Value);
            var status = machineStatus.Status.GetMachineStatus();

            if (status != Connect.Models.Enums.MachineStatus.Running)
                return null;

            var rrddataItems = await client.GetMachineRrddata(machine.ProxmoxId.Value);
            var lastData = rrddataItems.FirstOrDefault();

            if (lastData is null)
                return null;

            var machineHealthModel = new MachineHealthModel(long.Parse(lastData.MemoryTotal!),
                double.Parse(lastData.MemoryUsed!, CultureInfo.InvariantCulture), long.Parse(lastData.HDDTotal!),
                double.Parse(lastData.HDDUsed!, CultureInfo.InvariantCulture), double.Parse(lastData.CPUUsed!, CultureInfo.InvariantCulture));

            return machineHealthModel;
        }

        public async Task SendHeartBeatAsync(long machineId, string? initiatorUsername = null)
        {
            var machine = await _context.ProxmoxMachines.Include(m => m.Server).FirstOrDefaultAsync(x => x.Id == machineId && !x.IsDeleted);

            if (machine is null || !machine.ProxmoxId.HasValue)
                return;

            var credentials = GetServerCredentials(machine.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, machine.Server);

                var status = await proxmoxVirtualizationSystem.GetMachineStatus(machine.ProxmoxId.Value);

                await SendTelegramMachineNotify(machine.Server.Name, machine.Name, machine.Status, status.Status.GetMachineStatus());
                machine.Status = status.Status.GetMachineStatus();
            }
            catch
            {
                await SendTelegramMachineNotify(machine.Server.Name, machine.Name, machine.Status, Connect.Models.Enums.MachineStatus.Unknown);
                machine.Status = Connect.Models.Enums.MachineStatus.Unknown;
            }
            finally
            {
                _context.Update(machine);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BaseResult> TurnOffAsync(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, machine.Server);

                var result = await proxmoxVirtualizationSystem.ShutdownMachine(machine!.ProxmoxId!.Value);

                if (result.Success)
                {
                    machine.Status = Connect.Models.Enums.MachineStatus.Stopped;
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

        public async Task<BaseResult> TurnOnAsync(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, machine.Server);

                var result = await proxmoxVirtualizationSystem.StartMachine(machine!.ProxmoxId!.Value);

                if (result.Success)
                {
                    machine.Status = Connect.Models.Enums.MachineStatus.Running;
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

        public async Task<BaseResult> RebootAsync(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, machine.Server);

                var result = await proxmoxVirtualizationSystem.RebootMachine(machine!.ProxmoxId!.Value);

                return result;
            }
            catch (Exception ex)
            {
                return new(false, ex.ToString());
            }
        }

        public async Task<BaseResult> HardRebootAsync(long machineId, string? initiatorUsername = null)
        {
            var machine = await GetWithServerAsync(machineId);

            var validationResult = ValidateMachineBeforeAction(machine);

            if (validationResult is not null)
                return validationResult;

            var credentials = GetServerCredentials(machine!.Server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, machine.Server);

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
