namespace Tetas.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using DataEntities;

    //country implementation, all those implementations came from the interface 
    public class CountryRepository:ICountry
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Country> AddAsync(Country entity)
        {
            _context.Countries.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(Country entity)
        {
            _context.Countries.Remove(entity);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(Country entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Country> FindById(int key)
        {
            return await _context.Countries
                .FirstOrDefaultAsync(p => p.CountryId == key);
        }

        public Task<List<Country>> FindByClause(Func<Country, bool> selector = null)
        {
            var models = _context.Countries
                .Where(selector ?? (s => true));

            return Task.Run(() => models.ToList());
        }

        public Task<Country> GetByClause(Func<Country, bool> selector = null)
        {
            var models = _context.Countries
                .Where(selector ?? (s => true));

            return Task.Run(() => models.FirstOrDefault());
        }
    }
}
