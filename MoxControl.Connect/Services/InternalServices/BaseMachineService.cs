using Microsoft.Extensions.DependencyInjection;
using MoxControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Services.InternalServices
{
    public class BaseMachineService : BaseInternalService
    {

        public BaseMachineService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }
    }
}
