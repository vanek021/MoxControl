using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
