namespace Tetas.Domain.Entities
{
    using System;
    using Domain.Helpers;

    public class PostComment : BaseEntity, IBaseEntity
    {
        public string Body { get; set; }

        public DateTime Date { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ApplicationUser Owner { get; set; }

        public Post Post { get; set; }
    }
}
