using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Interfaces;

namespace MoxControl.Core.Repositories
{
    public abstract class AbstractDatabase : IDatabase
    {
        protected DbContext Context { get; }

        public AbstractDatabase(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            Context = context;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
