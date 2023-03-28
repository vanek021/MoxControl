using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Interfaces.Factories
{
    public interface IVirtualizationSystemClientFactory
    {
        public Task<IVirtualizationSystemClient> GetClientByVirtualizationSystemAsync(VirtualizationSystem type, string host, int port, string login, string password);
    }
}
