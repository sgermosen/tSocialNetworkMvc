using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tetas.Repositories.Implementations
{
    using Domain.Entities;
    using Infraestructure;
    using Contracts;
    using System;

    public class GroupRepository : Repository<Group>, IGroup
    {
        private readonly ApplicationDbContext _context;
        public GroupRepository(ApplicationDbContext context):base(context)//, IUserHelper userHelper) : base(context)
        {
            _context = context;
        }

        public IQueryable<Group> GetGroupWithPostsAndComments(string userid,long groupid)
        {
            var groups =  _context.Groups.Where(p=>p.Id==groupid && p.Deleted !=true)
                .Include(p => p.Privacy).Include(t => t.Type)
                .Include(gp => gp.GroupPosts).ThenInclude(gpc=>gpc.GroupPostComments).ThenInclude(gpcu=>gpcu.Owner)
                .Include(gp=>gp.GroupPosts).ThenInclude(gpu=>gpu.Owner)
                .Include(gu => gu.Owner);

            if (!string.IsNullOrEmpty(userid))
            {
                groups.Where(gu => gu.Owner.Id == userid);
            }

            return groups.AsNoTracking();
        }

        public IQueryable<Group> GetGroupWithPosts(string userid)
        {
            var groups = _context.Groups.Include(p => p.Privacy).Include(t => t.Type)
                .Include(gp => gp.GroupPosts).ThenInclude(gpu => gpu.Owner)
                .Include(gu => gu.Owner).Where(p => p.Deleted != true);

            if (!string.IsNullOrEmpty(userid))
            {
                groups = groups.Where(gu => gu.Owner.Id == userid);
            }

            return groups.AsNoTracking();
        }

       public IQueryable<Group> GetGroups(string userid)
        {
            var groups = _context.Groups
                .Include(p => p.Privacy).Include(g => g.Type)
                .Include(gu => gu.Owner)
                .Where(p => p.Deleted == false);

            if (!string.IsNullOrEmpty(userid))
            {
                groups= groups.Where(gu => gu.Owner.Id == userid);
            }

            return groups.AsNoTracking();
        }

        public async Task<Group> GetGroupWithPostsAndComments(long groupid)
        {
            var group = await _context.Groups.Where(p => p.Id == groupid && p.Deleted != true)
                .Include(p => p.Privacy).Include(t => t.Type)
                .Include(gp => gp.GroupPosts).ThenInclude(gpc => gpc.GroupPostComments).ThenInclude(gpcu => gpcu.Owner)
                .Include(gp => gp.GroupPosts).ThenInclude(gpu => gpu.Owner)
                .Include(gu => gu.Owner).FirstOrDefaultAsync();
            
            return group;
        }

        public async Task<GroupPost> AddPostAsync(GroupPost post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePostAsync(GroupPost post)
        {
            _context.GroupPosts.Remove(post);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdatePostAsync(GroupPost post)
        {
            _context.Update(post);
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

        public async Task<bool> PostExistAsync(long id)
        {
            return await _context.GroupPosts.AnyAsync(e => e.Id == id);
        }
    }

}
