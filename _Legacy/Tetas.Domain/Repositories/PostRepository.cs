namespace Tetas.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using DataEntities;

    public class PostRepository : IPost
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserPost> AddAsync(UserPost entity)
        {
            _context.UserPosts.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(UserPost entity)
        {
            _context.UserPosts.Remove(entity);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(UserPost entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<UserPost> FindById(int key)
        {
            return await _context.UserPosts
                .FirstOrDefaultAsync(p => p.UserPostId == key);
        }

        public Task<List<UserPost>> FindByClause(Func<UserPost, bool> selector = null)
        {
            var models = _context.UserPosts
               .Include(u => u.User)
               .Include(p => p.PostComments)
                .Where(selector ?? (s => true));

            return Task.Run(() => models.ToList());
        }

        public Task<UserPost> GetByClause(Func<UserPost, bool> selector = null)
        {
            var models = _context.UserPosts
                .Include(u => u.User)
                .Include(p => p.PostComments)
                 .Where(selector ?? (s => true));

            return Task.Run(() => models.FirstOrDefault());
        }
    }
}
