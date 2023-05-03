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
        private readonly IConnectServiceFactory _connectServiceFactory;

        public HangfireConnectManager(IConnectServiceFactory connectServiceFactory)
        {
            _connectServiceFactory = connectServiceFactory;
        }

        public async Task HangfireSendServerHeartBeat(VirtualizationSystem virtualizationSystem, long serverId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Servers.SendHeartBeat(serverId, initiatorUsername);
        }

        public async Task HangfireSendHeartBeatToAllServers()
        {
            var connectServices = _connectServiceFactory.GetAll();

            foreach (var connectService in connectServices)
            {
                var servers = await connectService.Item2.Servers.GetAllAsync();
                servers.ForEach(s => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.HangfireSendServerHeartBeat(connectService.Item1, s.Id, null)));
            }
        }

        public async Task HangfireSendMachineHeartBeat(VirtualizationSystem virtualizationSystem, long machineId, string? initiatorUsername = null)
        {
            var connectService = _connectServiceFactory.GetByVirtualizationSystem(virtualizationSystem);
            await connectService.Machines.SendHeartBeat(machineId, initiatorUsername);
        }

        public async Task HangfireSendHeartBeatToAllMachines()
        {
            var connectServices = _connectServiceFactory.GetAll();

            foreach (var connectService in connectServices)
            {
                var machines = await connectService.Item2.Machines.GetAllAsync();
                machines.ForEach(m => BackgroundJob.Enqueue<HangfireConnectManager>(h => h.HangfireSendMachineHeartBeat(connectService.Item1, m.Id, null)));
            }
        }

        public static void RegisterJobs()
        {
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.HangfireSendHeartBeatToAllServers(), Cron.Hourly());
            RecurringJob.AddOrUpdate<HangfireConnectManager>(x => x.HangfireSendHeartBeatToAllMachines(), Cron.Hourly());
        }
    }
}
