using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Interfaces
{
    public interface IVirtualizationSystemClientFactory
    {
        public IVirtualizationSystemClient GetClientByVirtualizationSystem(VirtualizationSystem type);
    }
}
