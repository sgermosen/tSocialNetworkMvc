namespace Tetas.Repositories.Implementations
{
    using Contracts;
    using Domain.Helpers;
    using Infraestructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
             

        public Task CreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            //Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TEntity>());
            ////or
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TEntity>());
            //var mapper = config.CreateMapper();
            // or


            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ExistAsync(long id)
        {
            return await _context.Set<TEntity>().AnyAsync(e => e.Id == id);
        }

        public bool Exists(long key)
        {
            return _context.Set<TEntity>().Any(e => e.Id == key);
        }

        public Task<List<TEntity>> FindByClause(Func<TEntity, bool> selector = null)
        {
            var models = _context.Set<TEntity>()
                .Where(selector ?? (s => true));

            return Task.Run(() => models.ToList());
        }

        public async Task<TEntity> FindByIdAsync(long key)
        {
            var entity = await _context.Set<TEntity>().FindAsync(key);
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            // return this.context.Set<T>().AsNoTracking();
            return _context.Set<TEntity>().AsNoTracking();
        }

        public Task<TEntity> GetByClause(Func<TEntity, bool> selector = null)
        {
            var models = _context.Set<TEntity>()
                .Where(selector ?? (s => true));

            return Task.Run(() => models.FirstOrDefault());
        }

        public async Task<TEntity> GetByIdAsync(long id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity;
        }

    
    }
}
