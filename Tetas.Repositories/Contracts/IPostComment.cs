using System.Linq;

namespace Tetas.Repositories.Contracts
{
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IPostComment : IRepository<PostComment>
    {
        //  IQueryable<Post> GetPostWithComments(string userid);

      

    }
     
}
