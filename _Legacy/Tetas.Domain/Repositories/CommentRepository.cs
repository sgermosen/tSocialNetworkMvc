namespace Tetas.Domain.Repositories
{
    using DataEntities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;

    //PostComment implementation, all those implementations came from the interface 
    public class CommentRepository : IComment
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PostComment> AddAsync(PostComment entity)
        {
            _context.PostComments.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(PostComment entity)
        {
            _context.PostComments.Remove(entity);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(PostComment entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<PostComment> FindById(int key)
        {
            return await _context.PostComments
                .FirstOrDefaultAsync(p => p.PostCommentId == key);
        }

        public Task<List<PostComment>> FindByClause(Func<PostComment, bool> selector = null)
        {
            var models = _context.PostComments
                .Where(selector ?? (s => true));

            return Task.Run(() => models.ToList());
        }

        public Task<PostComment> GetByClause(Func<PostComment, bool> selector = null)
        {
            var models = _context.PostComments
                 .Where(selector ?? (s => true));

            return Task.Run(() => models.FirstOrDefault());
        }
    }
}
