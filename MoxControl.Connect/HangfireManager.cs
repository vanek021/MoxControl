using Hangfire;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect
{
    public class HangfireConnectManager
    {
        private readonly IVirtualizationSystemClientFactory _virtualizationSystemClientFactory;
        private readonly IConnectServiceFactory _connectServiceFactory;

        public HangfireConnectManager(IVirtualizationSystemClientFactory virtualizationSystemClientFactory, IConnectServiceFactory connectServiceFactory)
        {
            _virtualizationSystemClientFactory = virtualizationSystemClientFactory;
            _connectServiceFactory = connectServiceFactory;
        }

        public void PerformBackgroundJob(VirtualizationSystem virtualizationSystem, Expression<Action<IConnectService>> methodCall)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            var action = methodCall.Compile();
            action.Invoke(connectService);
        }

        public async Task HangfireSendServerHeartBeat(VirtualizationSystem virtualizationSystem, long serverId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Servers.HangfireSendHeartBeat(serverId, initiatorUsername);
        }

        public async Task HangfireSyncMachines()
        {

        }
    }
}
