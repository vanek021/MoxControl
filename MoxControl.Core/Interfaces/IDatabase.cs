using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDKEY = System.Int64;

namespace MoxControl.Core.Interfaces
{
    public interface IReadableRepository<T> where T : class, IEntity
    {
        T GetById(IDKEY id);
        IEnumerable<T> GetManyByIds(IEnumerable<IDKEY> idKeysList);
        bool Contains(IDKEY id);
    }

    public interface IDatabase
    {
        void SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public interface IRepository
    {
        void SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
