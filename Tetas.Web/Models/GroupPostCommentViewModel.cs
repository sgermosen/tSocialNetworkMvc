namespace Tetas.Web.Models
{
    using Tetas.Domain.Entities;

    public class GroupPostCommentViewModel : GroupPostComment
    {
        public long PostId { get; set; }

        public long GroupId { get; set; }

        public string OwnerId { get; set; }
    }
}
