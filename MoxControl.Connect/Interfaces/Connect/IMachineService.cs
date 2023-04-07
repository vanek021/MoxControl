﻿using MoxControl.Connect.Models.Entities;
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
    }
}
