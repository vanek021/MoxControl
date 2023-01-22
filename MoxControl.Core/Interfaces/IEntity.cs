using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDKEY = System.Int64;

namespace MoxControl.Core.Interfaces
{
    public interface IEntity
    {
        IDKEY Id { get; set; }
    }
}
