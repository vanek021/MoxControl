using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoxControl.Core.Interfaces;

namespace MoxControl.Core.Repositories
{
    public abstract class BaseDbRepository<T> where T : class, IEntity
    {
        private DbContext _context;

        public BaseDbRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            _context = context;
        }

        #region Internals

        protected virtual DbSet<T> Table()
        {
            return _context.Set<T>();
        }

        // IQueryable methods should be always "protected"!
        protected virtual IQueryable<T> SingleWithIncludes()
        {
            return Table();
        }

        protected virtual IQueryable<T> ManyWithIncludes()
        {
            // By default, standard selector for full lists is equal to single item selector, you can optimize it
            return SingleWithIncludes();
        }

        protected virtual EntityEntry<T> EntityEntry(T entity)
        {
            return _context.Entry(entity);
        }

        #endregion
    }
}
