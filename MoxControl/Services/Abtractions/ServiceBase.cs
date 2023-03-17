using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Interfaces;
using MoxControl.Data;
using MoxControl.Services.Models;
using System.Linq.Expressions;

namespace MoxControl.Services.Abtractions
{
    public class ServiceBase<T> where T : class, IEntity
    {
        public ServiceBase(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public AppDbContext DbContext { get; init; }

        private DbSet<T> DbSet => DbContext.Set<T>();
        private bool isWaitingSave;

        public virtual ServiceResult<T> Add(T item)
        {
            DbSet.Add(item);
            if (!isWaitingSave)
            {
                SaveChanges();
            }
            var result = new ServiceResult<T>();
            return result;
        }
        public virtual IQueryable<T> Get()
        {
            return DbSet;
        }
        public virtual T GetSingle(Expression<Func<T, bool>> filter)
        {
            return Get().Single(filter);
        }
        public virtual T Get(params object[] primaryKey)
        {
            return DbSet.Find(primaryKey);
        }
        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
        {
            return await Get().SingleAsync(filter);
        }
        public virtual IList<T> Get(Expression<Func<T, bool>> filter)
        {
            return Get().Where(filter).ToList();
        }

        public virtual int Count(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                return Get().Where(filter).Count();
            }
            return Get().Count();
        }
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                return await Get().Where(filter).CountAsync();
            }
            return await Get().CountAsync();
        }
        public virtual ServiceResult<T> Update(T item)
        {
            DbSet.Update(item);
            if (!isWaitingSave)
            {
                SaveChanges();
            }
            var result = new ServiceResult<T>();
            return result;
        }
        public virtual ServiceResult<T> UpdateRange(params T[] items)
        {
            DbSet.UpdateRange(items);
            if (!isWaitingSave)
            {
                SaveChanges();
            }
            return new ServiceResult<T>();
        }
        public void Remove(params long[] primaryKey)
        {
            var item = Get(primaryKey);
            if (item != null)
            {
                Remove(item);
            }
        }
        public virtual void Remove(T item)
        {
            DbSet.Remove(item);
            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }
        public virtual void Remove(Expression<Func<T, bool>> filter)
        {
            DbSet.RemoveRange(DbSet.Where(filter));
            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }
        public virtual void RemoveRange(params T[] items)
        {
            DbSet.RemoveRange(items);
            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }

        public virtual void SaveChanges()
        {
            DbContext.SaveChanges();
            isWaitingSave = false;
        }

        public virtual async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
            isWaitingSave = false;
        }
    }
}
