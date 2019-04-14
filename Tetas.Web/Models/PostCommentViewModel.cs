namespace Tetas.Web.Models
{
    using Tetas.Domain.Entities;

    public class PostCommentViewModel : PostComment
    {
        public long PostId { get; set; }

        public string OwnerId { get; set; }
    }
}
