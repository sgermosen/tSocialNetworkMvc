using System.Linq;

namespace Tetas.Repositories.Contracts
{
    using Domain.Entities;

    public interface IPost : IRepository<Post>
    {
        IQueryable<Post> GetPostWithComments(string userid);

    }
     
}
