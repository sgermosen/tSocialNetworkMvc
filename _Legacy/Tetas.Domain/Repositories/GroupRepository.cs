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
    public class GroupRepository : IGroup
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Group> AddAsync(Group entity)
        {
            _context.Groups.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(Group entity)
        {
            _context.Groups.Remove(entity);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(Group entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Group> FindById(int key)
        {
            return await _context.Groups
                .FirstOrDefaultAsync(p => p.GroupId == key);
        }

        public Task<List<Group>> FindByClause(Func<Group, bool> selector = null)
        {
            var models = _context.Groups
                .Include(g => g.GroupType)
                .Include(g => g.User)
                .Include(u => u.GroupMembers)
                .Where(selector ?? (s => true));

            return Task.Run(() => models.ToList());
        }

        public Task<Group> GetByClause(Func<Group, bool> selector = null)
        {
            var models = _context.Groups
                 .Include(g => g.GroupType)
                 .Include(g => g.User)
                 .Include(u => u.GroupMembers)
                 .Where(selector ?? (s => true));

            return Task.Run(() => models.FirstOrDefault());
        }

        public Task<List<GroupMember>> GetMembers(Func<GroupMember, bool> selector = null)
        {
            var models = _context.GroupMembers
                .Include(g => g.User)
                .Include(u => u.Group)
               .Where(selector ?? (s => true));

            return Task.Run(() => models.ToList());

        }

        public Task<GroupMember> GetMemberByClause(Func<GroupMember, bool> selector = null)
        {
            var models = _context.GroupMembers
                .Include(g => g.User)
                .Include(u => u.Group)
               .Where(selector ?? (s => true));

            return Task.Run(() => models.FirstOrDefault());

        }

        public async Task<GroupMember> AddMemberAsync(GroupMember entity)
        {
            _context.GroupMembers.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }        
    }
}
