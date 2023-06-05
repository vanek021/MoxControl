using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;

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
            return ManyWithIncludes().ToListAsync();
        }

        public Task<int> GetCountAsync() 
        { 
            return ManyWithIncludes().CountAsync(); 
        }

        public Task<int> GetInitializedCountAsync()
        {
            return ManyWithIncludes()
                .Where(i => i.Status == ISOImageStatus.ReadyToUse)
                .CountAsync();
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
