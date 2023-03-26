using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Interfaces
{
    public interface IConnectService
    {
        Task Initialize(IServiceScopeFactory serviceScopeFactory);

        public Task<bool> CreateServerAsync(string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null);

        public Task<bool> UpdateServerAsync(long id, string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null);

        public Task<BaseServer?> GetServerAsync(long id);

        public Task<List<BaseServer>> GetAllServersAsync();
    }
}
