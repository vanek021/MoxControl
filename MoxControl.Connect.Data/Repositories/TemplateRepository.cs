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
    [Injectable(typeof(IReadableRepository<Template>))]
    public class TemplateRepository : WriteableRepository<Template>
    {
        public TemplateRepository(ConnectDbContext context) : base(context)
        {

        }

        public Task<List<Template>> GetAllAsync()
        {
            return Table().ToListAsync();
        }

        public Task<int> GetTotalCount()
        {
            return Table().CountAsync();
        }
    }
}
