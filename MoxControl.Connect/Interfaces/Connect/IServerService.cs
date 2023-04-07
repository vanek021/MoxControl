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

        public Task SendHeartBeat(long serverId, string? initiatorUsername = null);

        public Task SendHeartBeatToAll();

        public Task<ServerHealthModel?> GetHealthModel(long serverId, string? initiatorUsername = null);

        public Task<int> GetTotalCountAsync();

        public Task<int> GetAliveCountAsync();
    }
}
