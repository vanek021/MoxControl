using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;

namespace MoxControl.Connect.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<Template>))]
    public class TemplateRepository : WriteableRepository<Template>
    {
        public TemplateRepository(ConnectDbContext context) : base(context)
        {

        }

        protected override IQueryable<Template> SingleWithIncludes()
        {
            return base.SingleWithIncludes();
        }

        protected override IQueryable<Template> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<Template>> GetAllAsync()
        {
            return ManyWithIncludes().ToListAsync();
        }

        public Task<int> GetTotalCount()
        {
            return ManyWithIncludes().CountAsync();
        }

        public Task<int> GetInitializedCount()
        {
            return ManyWithIncludes()
                .Where(t => t.Status == TemplateStatus.ReadyToUse)
                .CountAsync();
        }

        public Task<Template?> GetByIdWithImageAsync(long id)
        {
            return ManyWithIncludes()
                .Include(t => t.ISOImage)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
