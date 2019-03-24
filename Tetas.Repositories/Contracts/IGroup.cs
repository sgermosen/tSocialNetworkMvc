using System.Linq;

namespace Tetas.Repositories.Contracts
{
    using Domain.Entities;

    public interface IGroup : IRepository<Group>
    {
        IQueryable<Group> GetGroupWithPostsAndComments(string userid, long groupid);

        IQueryable<Group> GetGroupWithPosts(string userid);

         IQueryable<Group> GetGroups(string userid);

    }
     
}
