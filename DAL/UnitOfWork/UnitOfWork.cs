using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork<TEntity, UEntity> : IUnitOfWork<TEntity, UEntity>, IDisposable
        where TEntity : class
        where UEntity : class
    {
        private readonly ForumContext _context;
        private Repository<TEntity> _tRepository;
        private Repository<UEntity> _uRepository;
        private bool _disposed;

        public UnitOfWork(ForumContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IRepository<TEntity> TRepository
        {
            get
            {
                return _tRepository ?? (_tRepository = new Repository<TEntity>(_context));
            }
        }

        public IRepository<UEntity> URepository
        {
            get
            {
                return _uRepository ?? (_uRepository = new Repository<UEntity>(_context));
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
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
                    if (_tRepository != null)
                        _tRepository.Dispose();

                    if (_uRepository != null)
                        _uRepository.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
