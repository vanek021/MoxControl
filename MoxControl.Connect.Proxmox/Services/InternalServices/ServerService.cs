using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;
using MoxControl.Connect.Proxmox.Models.Enums;
using MoxControl.Connect.Proxmox.VirtualizationClient.DTOs;
using MoxControl.Connect.Proxmox.VirtualizationClient.Helpers;
using MoxControl.Connect.Services.InternalServices;
using MoxControl.Infrastructure.Extensions;
using System.Globalization;

namespace MoxControl.Connect.Proxmox.Services.InternalServices
{
    public class ServerService : BaseServerService, IServerService
    {
        private readonly ConnectProxmoxDbContext _context;

        public ServerService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<ConnectProxmoxDbContext>();
        }

        #region lambdas

        public async Task<BaseServer?> GetAsync(long id)
            => await GetBaseQuery().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<BaseServer>> GetAllAsync()
            => await GetBaseQuery().Select(x => (BaseServer)x).ToListAsync();

        public async Task<int> GetTotalCountAsync()
            => await GetBaseQuery().CountAsync();

        public async Task<int> GetAliveCountAsync()
            => await GetBaseQuery().Where(x => x.Status == ServerStatus.Running).CountAsync();

        private IQueryable<ProxmoxServer> GetBaseQuery()
            => _context.ProxmoxServers.Where(x => !x.IsDeleted);

        #endregion

        public async Task<bool> CreateAsync(string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null, string? initiatorUsername = null)
        {
            var server = new ProxmoxServer()
            {
                Host = host,
                Port = port,
                AuthorizationType = authorizationType,
                Name = name,
                Description = description,
                RootLogin = rootLogin,
                RootPassword = rootPassword
            };

            _context.ProxmoxServers.Add(server);

            try
            {
                await _context.SaveChangesAsync();
                BackgroundJob.Enqueue<HangfireConnectManager>(x => x.SendServerHeartBeatAsync(VirtualizationSystem.Proxmox, server.Id, initiatorUsername));
                return true;
            }
            catch (Exception ex)
            {
                await _generalNotificationService.AddInternalServerErrorAsync(ex);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(long id, string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (server is null)
                return false;

            server.Host = host;
            server.Port = port;
            server.AuthorizationType = authorizationType;
            server.Name = name;
            server.Description = description;
            server.RootLogin = rootLogin;
            server.RootPassword = rootPassword;

            _context.ProxmoxServers.Update(server);

            try
            {
                await _context.SaveChangesAsync();
                BackgroundJob.Enqueue<HangfireConnectManager>(x => x.SendServerHeartBeatAsync(VirtualizationSystem.Proxmox, server.Id, initiatorUsername));
                return true;
            }
            catch (Exception ex)
            {
                await _generalNotificationService.AddInternalServerErrorAsync(ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (server is null)
                return false;

            server.IsDeleted = true;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _generalNotificationService.AddInternalServerErrorAsync(ex);
                return false;
            }
        }

        public async Task SendHeartBeatAsync(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);

            if (server is null)
                return;

            var credentials = GetServerCredentials(server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, server);

                server.Status = ServerStatus.Running;
            }
            catch
            {
                await SendTelegramServerNotify(server.Name, server.Status, ServerStatus.Unknown);
                server.Status = ServerStatus.Unknown;
            }
            finally
            {
                _context.Update(server);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendHeartBeatToAllAsync()
        {
            var serverIds = await _context.ProxmoxServers.Where(x => !x.IsDeleted).Select(x => x.Id).ToListAsync();

            foreach (var serverId in serverIds)
                await SendHeartBeatAsync(serverId);
        }

        public async Task SyncMachinesAsync(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers
                .Include(s => s.Machines)
                .FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);

            if (server is null)
                return;

            var credentials = GetServerCredentials(server, initiatorUsername);

            List<MachineListItem> machines = new();

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, server);
                machines = await proxmoxVirtualizationSystem.GetNodeMachines();
            }
            catch (Exception ex)
            {
                await _generalNotificationService.AddInternalServerErrorAsync(ex);
                return;
            }

            foreach (var machine in machines.Where(m => m.Template == default))
            {
                var dbMachine = server.Machines.FirstOrDefault(m => m.ProxmoxId.HasValue && m.ProxmoxId.Value == machine.VMid);

                if (dbMachine is not null)
                {
                    dbMachine.Name = machine.Name ?? dbMachine.Name;
                    dbMachine.Description ??= $"Proxmox Machine {machine.VMid}";
                    dbMachine.ProxmoxName = machine.Name;
                    dbMachine.Status = machine.Status.GetMachineStatus();
                    dbMachine.Stage = MachineStage.Using;
                    dbMachine.CPUCores = machine.CPUs;
                    dbMachine.RAMSize = Convert.ToInt32(machine.MemoryTotal / (1024 * 1024));
                    dbMachine.HDDSize = Convert.ToInt32(machine.HDDTotal / (1024 * 1024 * 1024));
                    _context.ProxmoxMachines.Update(dbMachine);

                }
                else
                {
                    var newMachine = new ProxmoxMachine()
                    {
                        Name = machine.Name ?? $"Proxmox Machine {machine.VMid}",
                        Description = $"Proxmox Machine {machine.VMid}",
                        Status = machine.Status.GetMachineStatus(),
                        ProxmoxName = machine.Name,
                        ProxmoxId = machine.VMid,
                        Stage = MachineStage.Using,
                        CPUCores = machine.CPUs,
                        RAMSize = Convert.ToInt32(machine.MemoryTotal / (1024 * 1024)),
                        HDDSize = Convert.ToInt32(machine.HDDTotal / (1024 * 1024 * 1024)),
                        ServerId = serverId
                    };
                    _context.ProxmoxMachines.Add(newMachine);
                }
                await _context.SaveChangesAsync();
            }

            server.LastMachinesSync = DateTime.UtcNow.ToMoscowTime();
            _context.ProxmoxServers.Update(server);
            await _context.SaveChangesAsync();
        }

        public async Task<ServerHealthModel?> GetHealthModelAsync(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);

            if (server is null)
                return null;

            var credentials = GetServerCredentials(server, initiatorUsername);

            var client = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, server);

            var rrddataItems = await client.GetServerRrddata();
            var lastData = rrddataItems.LastOrDefault();

            if (lastData is null)
                return null;

            var serverHealthModel = new ServerHealthModel(long.Parse(lastData.MemoryTotal!),
                double.Parse(lastData.MemoryUsed!, CultureInfo.InvariantCulture), long.Parse(lastData.HDDTotal!),
                double.Parse(lastData.HDDUsed!, CultureInfo.InvariantCulture), double.Parse(lastData.CPUUsed!, CultureInfo.InvariantCulture));

            return serverHealthModel;
        }

        public async Task UploadImageAsync(long serverId, long imageId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers
                .FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);
            var image = await _imageManager.GetByIdAsync(imageId);

            if (server is null || image is null || (server.ImageData is not null && server.ImageData.ImageIds.Contains(imageId)))
                return;

            var credentials = GetServerCredentials(server, initiatorUsername);

            var client = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, server);

            var imageName = Path.GetFileName(image.ImagePath);
            var imagePath = image.StorageMethod == ImageStorageMethod.Local ? $"{_configuration["BaseUrl"]}{image.ImagePath}" : image.ImagePath;

            await client.InsertImage(imageName, imagePath);

            server.ImageData ??= new ImageData();
            
            server.ImageData.ImageIds.Add(imageId);

            _context.ProxmoxServers.Update(server);
            await _context.SaveChangesAsync();
        }

        public async Task HandleCreateTemplateAsync(long templateId, string? initiatorUsername = null)
        {
            var template = await _templateManager.GetByIdWithImageAsync(templateId);
            var servers = await GetBaseQuery().ToListAsync();

            if (template is null)
                return;

            foreach (var server in servers)
            {
                try
                {
                    await HandleCreateTemplateForServer(server, template);

                    server.TemplateData ??= new TemplateData();
                    server.TemplateData.TemplateIds.Add(templateId);

                    _context.ProxmoxServers.Update(server);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    await _generalNotificationService.AddInternalServerErrorAsync(ex);
                }
            }
        }

        private async Task HandleCreateTemplateForServer(ProxmoxServer proxmoxServer, Template template, string? initiatorUsername = null)
        {
            var credentials = GetServerCredentials(proxmoxServer, initiatorUsername);

            var client = new ProxmoxVirtualizationClient(credentials.Login, credentials.Password, proxmoxServer);

            var status = await client.CreateTemplateMachine(template.Name, Path.GetFileName(template.ISOImage.ImagePath),
                template.CPUSockets, template.CPUCores, template.RAMSize, template.HDDSize, proxmoxServer.BaseDisksStorage, proxmoxServer.BaseStorage);

            var templateMachine = new TemplateMachine()
            {
                CPUCores = template.CPUCores,
                RAMSize = template.RAMSize,
                HDDSize = template.HDDSize,
                CPUSockets = template.CPUSockets,
                Status = TemplateMachineStatus.Converted,
                Name = template.Name,
                ProxmoxId = (int)status.VMid,
                ProxmoxName = status.Name,
                ServerId = proxmoxServer.Id,
                TemplateId = template.Id,
                Description = $"Template machine for Template: {template.Name}"
            };

            _context.Add(templateMachine);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ISOImage>> GetAvailableImagesAsync(long serverId)
        {
            var server = await GetBaseQuery().FirstOrDefaultAsync(s => s.Id == serverId);

            if (server is null || server.ImageData is null)
                return new();

            var images = await _imageManager.GetAllAsync();
            return images.Where(i => server.ImageData.ImageIds.Contains(i.Id)).ToList();
        }

        public async Task<string?> GetWebUISourceAsync(long serverId)
        {
            var server = await GetAsync(serverId);

            if (server is null)
                return null;

            return $"https://{server.Host}:{server.Port}";
        }
    }
}
