using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Tetas.Repositories.Implementations
{
    using Contracts;
    using Domain.Entities;
    using Infraestructure;
    using System;
    using System.Collections.Generic;
    using Tetas.Common.ViewModels;

    public class GroupRepository : Repository<Group>, IGroup
    {
        private readonly ApplicationDbContext _context;
        public GroupRepository(ApplicationDbContext context) : base(context)//, IUserHelper userHelper) : base(context)
        {
            _context = context;
        }

        public IQueryable<Group> GetGroupWithPostsAndComments(string userid, long groupid)
        {
            var groups = _context.Groups.Where(p => p.Id == groupid && p.Deleted != true)
                .Include(p => p.Privacy).Include(t => t.Type)
                .Include(gp => gp.GroupPosts).ThenInclude(gpc => gpc.GroupPostComments).ThenInclude(gpcu => gpcu.Owner)
                .Include(gp => gp.GroupPosts).ThenInclude(gpu => gpu.Owner)
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

        public async Task<ICollection<GroupModel>> GetPublicAndMyGroupsAsync(string userid)
        {
            var groups = await _context.Groups
                .Include(p => p.GroupMembers)
                .Include(pr=>pr.Privacy)
                .Include (o=>o.Owner)
                .Where(p => p.Deleted != true).ToListAsync();

            var myGroups = new List<GroupModel>();
            var ban = false;
            var stat = false;
            var applied = false;
            var isAdmin = false;
            GroupMember memb;
            foreach (var item in groups)
            {
                if (item.Privacy.Id == 1 ||
                    _context.GroupMembers.Any(p => p.Group.Id == item.Id && p.User.Id == userid) ||
                    item.Owner.Id == userid)
                {
                    memb = _context.GroupMembers
                       .Where(p => p.Group.Id == item.Id && p.User.Id == userid)
                       .FirstOrDefault();

                    if (item.Owner.Id==userid)
                    {
                        isAdmin = true;
                    }
                    else
                    {
                        isAdmin = false;
                    }
                    if (memb != null)
                    {
                        ban = memb.Banned;
                        stat = memb.State;
                        applied = memb.Applied;
                    }
                    else
                    {
                          ban = false;
                          stat = false;
                          applied = false; 
                    }

                    myGroups.Add(new GroupModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Link = item.Link,
                        PictureUrl = item.PictureUrl,
                        CreationDate = item.CreationDate,
                        Type = item.Type,
                        Privacy = item.Privacy,
                        State = stat,
                        Banned = ban,
                        Applied = applied,
                        IsAdmin= isAdmin
                    });
                }

            }
            return myGroups;
            //return (from item in groups
            //        where
            //        item.Privacy.Id == 1 ||
            //        item.GroupMembers.Any(p => p.Group.Id == item.Id && p.User.Id == userid) ||
            //        item.Owner.Id == userid
            //        select new GroupModel
            //        {
            //            Id = item.Id,
            //            Name = item.Name,
            //            Description = item.Description,
            //            Link = item.Link,
            //            PictureUrl = item.PictureUrl,
            //            CreationDate = item.CreationDate,
            //            Type = item.Type,
            //            Privacy = item.Privacy,
            //            State = item.GroupMembers.Any(p => p.Group.Id == item.Id && p.User.Id == userid && p.State == true),
            //            Banned = item.GroupMembers.Any(p => p.Group.Id == item.Id && p.User.Id == userid && p.Banned == true)
            //        }).ToList();
        }

        public IQueryable<Group> GetGroups(string userid)
        {
            var groups = _context.Groups
                .Include(p => p.Privacy).Include(g => g.Type)
                .Include(gu => gu.Owner)
                .Where(p => p.Deleted == false);

            if (!string.IsNullOrEmpty(userid))
            {
                groups = groups.Where(gu => gu.Owner.Id == userid);
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
