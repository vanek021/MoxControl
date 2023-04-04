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
    [Injectable(typeof(IReadableRepository<ISOImage>))]
    public class ISOImageRepository : WriteableRepository<ISOImage>
    {
        public ISOImageRepository(ConnectDbContext context) : base(context)
        {

        }

        public Task<List<ISOImage>> GetAll()
        {
            return Table().ToListAsync();
        }

        public Task<string?> GetImagePath(long id)
        {
            return Table()
                .Where(x => x.Id == id)
                .Select(x => x.ImagePath)
                .FirstOrDefaultAsync();
        }
    }
}
