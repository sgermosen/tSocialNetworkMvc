namespace Tetas.Repositories.Implementations
{
    using Contracts;
    using Domain.Entities;
    using Domain.Helpers;
    using Infraestructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
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
                .Include(p => p.Reactions)
                .Include(p => p.Owner).Where(p => !p.Deleted);

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
                .Include(p => p.Reactions)
                .Include(p => p.Owner).Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ReactionSummary> ToggleReactionAsync(long postId, string userId, ReactionType type)
        {
            var existing = await _context.Reactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.OwnerId == userId);

            if (existing == null)
            {
                _context.Reactions.Add(new Reaction
                {
                    PostId = postId,
                    OwnerId = userId,
                    Type = type,
                    Date = DateTime.UtcNow
                });
            }
            else if (existing.Type == type)
            {
                _context.Reactions.Remove(existing);
            }
            else
            {
                existing.Type = type;
                existing.Date = DateTime.UtcNow;
                _context.Reactions.Update(existing);
            }

            await _context.SaveChangesAsync();

            return await GetReactionSummaryAsync(postId, userId);
        }

        public async Task<ReactionSummary> GetReactionSummaryAsync(long postId, string userId)
        {
            var reactions = await _context.Reactions
                .Where(r => r.PostId == postId)
                .AsNoTracking()
                .ToListAsync();

            var summary = new ReactionSummary
            {
                Total = reactions.Count,
                ByType = reactions
                    .GroupBy(r => r.Type)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            if (!string.IsNullOrEmpty(userId))
            {
                var mine = reactions.FirstOrDefault(r => r.OwnerId == userId);
                if (mine != null)
                {
                    summary.MyReaction = mine.Type;
                }
            }

            return summary;
        }

        public async Task<PostComment> GetPostCommentByIdAsync(long id)
        {
            return await _context.PostComments.Include(p => p.Post).ThenInclude(pu => pu.Owner)
                .Include(p => p.Owner).Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PostComment> AddCommentAsync(PostComment comment)
        {
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(PostComment comment)
        {
            //_context.PostComments.Remove(comment);
            //int result = await _context.SaveChangesAsync();
            //return result > 0;
            comment.Deleted = true;
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
