using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tetas.Repositories.Implementations
{
    using Domain.Entities;
    using Infraestructure;
    using Contracts;

    public class GroupRepository : Repository<Group>, IGroup
    {
        private readonly ApplicationDbContext _context;
        public GroupRepository(ApplicationDbContext context):base(context)//, IUserHelper userHelper) : base(context)
        {
            _context = context;
        }

        public IQueryable<Group> GetGroupWithPostsAndComments(string userid,long groupid)
        {
            var groups = _context.Groups.Where(p=>p.Id==groupid)
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
            var groups = _context.Groups
                .Include(gp => gp.GroupPosts).ThenInclude(gpu => gpu.Owner)
                .Include(gu => gu.Owner);

            if (!string.IsNullOrEmpty(userid))
            {
                groups.Where(gu => gu.Owner.Id == userid);
            }

            return groups.AsNoTracking();
        }


    }

}
