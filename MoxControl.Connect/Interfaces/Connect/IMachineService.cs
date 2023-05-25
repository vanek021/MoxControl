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
        public Task<List<BaseMachine>> GetAllByServerAsync(long serverId);

        public Task<List<BaseMachine>> GetAllWithTemplateAsync();

        public Task<int> GetTotalCountAsync();

        public Task<int> GetAliveCountAsync();

        public Task<bool> CreateAsync(BaseMachine machine, long serverId, long? templateId = null, long? imageId = null, string? initiatorUsername = null);

        public Task<bool> UpdateAsync(BaseMachine machine);

        public Task<bool> DeleteAsync(long id);

        public Task<BaseMachine?> GetAsync(long id);

        public Task<MachineHealthModel?> GetHealthModelAsync(long machineId, string? initiatorUsername = null);

        public Task SendHeartBeatAsync(long machineId, string? initiatorUsername = null);

        public Task<List<BaseMachine>> GetAllAsync();

        public Task<BaseResult> TurnOffAsync(long machineId, string? initiatorUsername = null);

        public Task<BaseResult> TurnOnAsync(long machineId, string? initiatorUsername = null);

        public Task<BaseResult> RebootAsync(long machineId, string? initiatorUsername = null);

        public Task<BaseResult> HardRebootAsync(long machineId, string? initiatorUsername = null);

        public Task<string?> GetConsoleSourceAsync(long id);

        public Task ProcessCreateAsync(long machineId, string? initiatorUsername = null);
    }
}
