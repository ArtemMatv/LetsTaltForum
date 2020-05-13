using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        Task Remove(int id);
        void Remove(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetWithRawSql(string query,
            params object[] parameters);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
        Task<int> SaveChangesAsync();
    }
}
