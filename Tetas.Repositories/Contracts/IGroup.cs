namespace Tetas.Repositories.Contracts
{
    using Domain.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Tetas.Common.ViewModels;

    public interface IGroup : IRepository<Group>
    {
        IQueryable<Group> GetGroupWithPostsAndComments(string userid, long groupid);

        IQueryable<Group> GetGroupWithPosts(string userid);

        IQueryable<Group> GetGroups(string userid);

        Task<ICollection<GroupModel>> GetPublicAndMyGroupsAsync(string userid);

        Task<Group> GetGroupWithPostsAndComments(long groupid);

        Task<GroupPost> AddPostAsync(GroupPost comment);

        Task<bool> DeletePostAsync(GroupPost comment);

        Task<bool> UpdatePostAsync(GroupPost comment);

        Task<bool> PostExistAsync(long id);

        Task<GroupPost> GetPostByIdAsync(long id);

        Task<GroupPostComment> GetPostCommentByIdAsync(long id);

        Task<GroupPostComment> AddCommentAsync(GroupPostComment comment);

        Task<bool> DeleteCommentAsync(GroupPostComment comment);

        Task<bool> UpdateCommentAsync(GroupPostComment comment);

        Task<bool> CommentExistAsync(long id);

    }

}
