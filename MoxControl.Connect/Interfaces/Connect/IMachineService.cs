using MoxControl.Connect.Models.Entities;
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
    }
}
