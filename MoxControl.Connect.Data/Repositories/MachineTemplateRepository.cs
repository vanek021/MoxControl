using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Models.Entities;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<MachineTemplate>))]
    public class MachineTemplateRepository : WriteableRepository<MachineTemplate>
    {
        public MachineTemplateRepository(DbContext context) : base(context)
        {

        }
    }
}
