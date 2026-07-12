namespace Tetas.Domain.Entities
{
    using System;

    public class UserBlock
    {
        public long Id { get; set; }

        public string BlockerId { get; set; }

        public ApplicationUser Blocker { get; set; }

        public string BlockedId { get; set; }

        public ApplicationUser Blocked { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
