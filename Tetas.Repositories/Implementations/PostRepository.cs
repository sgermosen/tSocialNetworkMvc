namespace Tetas.Repositories.Implementations
{
    using Contracts;
    using Domain.Entities;
    using Infraestructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class PostRepository : Repository<Post>, IPost
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)//, IUserHelper userHelper) : base(context)
        {
            _context = context;
        }

        public IQueryable<Post> GetPostWithComments(string userid)
        {
            var posts = _context.Posts
                .Include(p => p.PostComments).ThenInclude(u => u.Owner)
                .Include(p => p.Owner).Where(p => p.Deleted != true);

            if (!string.IsNullOrEmpty(userid))
            {
                posts = posts.Where(p => p.Owner.Id == userid);
            }

            return posts.AsNoTracking().OrderByDescending(p => p.Date);
        }

        public async Task<Post> GetPostByIdAsync(long id)
        {
            return await _context.Posts
                .Include(p => p.PostComments).ThenInclude(u => u.Owner)
                .Include(p => p.Owner).Where(p => p.Id == id && p.Deleted != true).FirstOrDefaultAsync();
        }

        public async Task<PostComment> GetPostCommentByIdAsync(long id)
        {
            return await _context.PostComments.Include(p => p.Post).ThenInclude(pu => pu.Owner)
                .Include(p => p.Owner).Where(p => p.Id == id && p.Deleted != true).FirstOrDefaultAsync();
        }

        public async Task<PostComment> AddCommentAsync(PostComment comment)
        {
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(PostComment comment)
        {
            _context.PostComments.Remove(comment);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateCommentAsync(PostComment comment)
        {
            _context.Update(comment);
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

        public async Task<bool> CommentExistAsync(long id)
        {
            return await _context.PostComments.AnyAsync(e => e.Id == id);
        }
    }

}
