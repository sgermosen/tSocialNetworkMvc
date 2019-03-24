namespace Tetas.Domain.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    //generic interface repository, always keep it generic, 
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> Delete(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<TEntity> FindById(TKey key);
        Task<List<TEntity>> FindByClause(Func<TEntity, bool> selector = null);
        Task<TEntity> GetByClause(Func<TEntity, bool> selector = null);


    }
}
