namespace Tetas.Repositories.Contracts
{
    using Domain.Entities;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IPost : IRepository<Post>
    {
        IQueryable<Post> GetPostWithComments(string userid);

        Task<Post> GetPostByIdAsync(long id);

        Task<PostComment> GetPostCommentByIdAsync(long id);

        Task<PostComment> AddCommentAsync(PostComment comment);

        Task<bool> DeleteCommentAsync(PostComment comment);

        Task<bool> UpdateCommentAsync(PostComment comment);

        Task<bool> CommentExistAsync(long id);

    }

}
