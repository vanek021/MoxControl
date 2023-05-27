using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Interfaces;
using IDKEY = System.Int64;

namespace MoxControl.Core.Repositories
{
    public class WriteableRepository<T> : ReadOnlyRepository<T> where T : class, IEntity
    {
        public WriteableRepository(DbContext context) : base(context)
        {
        }

        public virtual void Insert(T entity)
        {
            Table().Add(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry(entity).State = EntityState.Modified;
        }

        public virtual void InsertOrUpdate(T entity)
        {
            if (Contains(entity))
                Update(entity);
            else
                Insert(entity);
        }

        public virtual void Delete(IDKEY id)
        {
            T entity = Table().Find(id);
            Table().Remove(entity);
        }

        public virtual void Delete(T entity)
        {
            Table().Remove(entity);
        }
    }
}
