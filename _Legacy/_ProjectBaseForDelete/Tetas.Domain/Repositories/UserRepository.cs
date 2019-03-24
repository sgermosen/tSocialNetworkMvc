namespace Tetas.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using DataEntities;

    public class UserRepository : IUser
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<User> AddAsync(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(User entity)
        {
            _context.Users.Remove(entity);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public Task<User> FindById(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindById(string key)
        {
            return await _context.Users
                .FirstOrDefaultAsync(p => p.UserId == key);
        }

        public Task<List<User>> FindByClause(Func<User, bool> selector = null)
        {
            var models = _context.Users
               .Include(p => p.Country)
               .Include(p => p.Gender)
                .Where(selector ?? (s => true));

            return Task.Run(() => models.ToList());
        }

        public Task<User> GetByClause(Func<User, bool> selector = null)
        {
            var models = _context.Users
                .Include(p => p.Country)
                .Include(p => p.Gender)
                 .Where(selector ?? (s => true));

            return Task.Run(() => models.FirstOrDefault());
        }
    }
}
