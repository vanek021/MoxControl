using MoxControl.Connect.Enums;
using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models
{
    public class BaseServer : BaseRecord
    {
        public virtual VirtualizationSystem VirtualizationSystem { get; set; }
    }
}
