using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Interfaces;
using IDKEY = System.Int64;

namespace MoxControl.Core.Repositories
{
    public class ReadOnlyRepository<T> : BaseDbRepository<T>, IReadableRepository<T> where T : class, IEntity
    {
        public ReadOnlyRepository(DbContext context) : base(context)
        {
        }

        public virtual T GetById(IDKEY id)
        {
            return SingleWithIncludes().FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<T> GetManyByIds(IEnumerable<IDKEY> idKeysList)
        {
            return ManyWithIncludes()
                .Where(x => idKeysList.Contains(x.Id))
                .ToList();
        }

        public virtual bool Contains(IDKEY id)
        {
            return Table().Any(x => x.Id == id);
        }

        public virtual bool Contains(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return Table().Any(x => x.Id == entity.Id);
        }

        #region Async

        public virtual Task<T> GetByIdAsync(IDKEY id)
        {
            return SingleWithIncludes().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual IAsyncEnumerable<T> GetManyByIdsAsync(IEnumerable<IDKEY> idKeysList)
        {
            return ManyWithIncludes()
                .Where(x => idKeysList.Contains(x.Id))
                .AsAsyncEnumerable();
        }

        public virtual Task<bool> ContainsAsync(IDKEY id)
        {
            return Table().AnyAsync(x => x.Id == id);
        }

        public virtual Task<bool> ContainsAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return Table().AnyAsync(x => x.Id == entity.Id);
        }

        #endregion

    }
}
