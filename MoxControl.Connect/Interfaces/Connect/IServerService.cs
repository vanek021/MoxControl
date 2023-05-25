using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Interfaces.Connect
{
    public interface IServerService
    {
        public Task<bool> CreateAsync(string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null, string? initiatorUsername = null);

        public Task<bool> UpdateAsync(long id, string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null, string? initiatorUsername = null);

        public Task<bool> DeleteAsync(long id);

        public Task<BaseServer?> GetAsync(long id);

        public Task<List<BaseServer>> GetAllAsync();

        public Task SendHeartBeatAsync(long serverId, string? initiatorUsername = null);

        public Task SendHeartBeatToAllAsync();

        public Task<ServerHealthModel?> GetHealthModelAsync(long serverId, string? initiatorUsername = null);

        public Task<int> GetTotalCountAsync();

        public Task<int> GetAliveCountAsync();

        public Task UploadImageAsync(long serverId, long imageId, string? initiatorUsername = null);

        public Task SyncMachinesAsync(long serverId, string? initiatorUsername = null);

        public Task HandleCreateTemplateAsync(long templateId, string? initiatorUsername = null);

        public Task<List<ISOImage>> GetAvailableImagesAsync(long serverId);
    }
}
