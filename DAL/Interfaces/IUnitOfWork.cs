using System;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork<TEntity, UEntity> : IDisposable
        where TEntity : class
        where UEntity : class
    {
        public IRepository<TEntity> TRepository { get; }
        public IRepository<UEntity> URepository { get; }
        public Task Commit();
    }
}
