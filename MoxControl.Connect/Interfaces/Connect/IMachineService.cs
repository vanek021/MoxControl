using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Interfaces.Connect
{
    public interface IMachineService
    {
        public Task<List<BaseMachine>> GetAllByServer(long serverId);

        public Task<List<BaseMachine>> GetAllWithTemplate();

        public Task<int> GetTotalCountAsync();

        public Task<int> GetAliveCountAsync();

        public Task<bool> CreateAsync(BaseMachine machine, long serverId, long? templateId = null);

        public Task<bool> UpdateAsync(BaseMachine machine);

        public Task<bool> DeleteAsync(long id);

        public Task<BaseMachine?> GetAsync(long id);

        public Task<MachineHealthModel?> GetHealthModel(long machineId, string? initiatorUsername = null);

        public Task SendHeartBeat(long machineId, string? initiatorUsername = null);

        public Task<List<BaseMachine>> GetAllAsync();

        public Task<BaseResult> TurnOff(long machineId, string? initiatorUsername = null);

        public Task<BaseResult> TurnOn(long machineId, string? initiatorUsername = null);

        public Task<BaseResult> Reboot(long machineId, string? initiatorUsername = null);

        public Task<BaseResult> HardReboot(long machineId, string? initiatorUsername = null);

        public Task<string?> GetConsoleSourceAsync(long id);
    }
}
