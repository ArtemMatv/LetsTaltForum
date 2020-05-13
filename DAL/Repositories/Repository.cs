using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    /// <summary>
    /// Not usable for user classes. For users use UserRepository
    /// </summary>
    /// <typeparam name="TEntity">Entity to create a repository</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly ForumContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private bool _disposed;

        public Repository(ForumContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _disposed = false;
        }

        public async Task Remove(int id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);

            if (entityToDelete == null)
                throw new ArgumentException($"There is no such {typeof(TEntity)}");

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetWithRawSql(string query, params object[] parameters)
        {
            return await _dbSet.FromSqlRaw(query, parameters).ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            //_dbSet.Attach(entityToUpdate);
            
            _dbSet.Update(entityToUpdate);

            //_context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public void Remove(TEntity entity)
        {
            if (!_dbSet.Contains(entity)
                || entity == null)
                throw new ArgumentException($"There is no such {typeof(TEntity)}");

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }
    }
}
