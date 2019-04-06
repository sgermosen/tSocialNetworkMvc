using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tetas.Repositories.Implementations
{
    using Domain.Entities;
    using Infraestructure;
    using Contracts;
    using System.Threading.Tasks;

    public class PostCommentRepository : Repository<PostComment>, IPostComment
    {
        private readonly ApplicationDbContext _context;
        public PostCommentRepository(ApplicationDbContext context) : base(context)//, IUserHelper userHelper) : base(context)
        {
            _context = context;
        }

        //public IQueryable<Post> GetPostWithComments(string userid)
        //{
        //    var posts = _context.Posts
        //        .Include(p => p.PostComments).ThenInclude(u => u.Owner)
        //        .Include(p => p.Owner).Where(p => p.Deleted != true);

        //    if (!string.IsNullOrEmpty(userid))
        //    {
        //        posts = posts.Where(p => p.Owner.Id == userid);
        //    }

        //    return posts.AsNoTracking().OrderByDescending(p => p.Date);
        //}

        
    }

}
