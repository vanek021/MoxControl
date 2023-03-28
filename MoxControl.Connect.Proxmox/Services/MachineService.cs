using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Services
{
    public class MachineService : IMachineService
    {
        public MachineService(IServiceScopeFactory serviceScopeFactory) 
        {
        
        }
    }
}
