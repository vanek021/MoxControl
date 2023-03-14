using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Interfaces
{
    public interface IVirtualizationSystemClient
    {
        public Task Initialize(IServiceScopeFactory scopeFactory);
    }
}
