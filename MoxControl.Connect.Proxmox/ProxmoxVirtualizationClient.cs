using MoxControl.Connect.Interfaces;
using Corsinvest.ProxmoxVE.Api;
using Microsoft.Extensions.DependencyInjection;

namespace MoxControl.Connect.Proxmox
{
    public class ProxmoxVirtualizationClient : IVirtualizationSystemClient
    {
        private PveClient _pveClient;

        public async Task Initialize(IServiceScopeFactory scopeFactory, string host, int port, string login, string password)
        {
            _pveClient = new PveClient(host, port);
            var authResult = await _pveClient.Login(login, password);

            if (!authResult)
                throw new Exception($"Неудачная попытка авторизации с сервером Proxmox: {host}:{port}");
        }
    }
}