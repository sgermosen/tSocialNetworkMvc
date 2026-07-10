namespace Tetas.Domain.Entities
{
    using System;
    using Domain.Helpers;

    public class Reaction
    {
        public long Id { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public string OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }

        public ReactionType Type { get; set; }

        public DateTime Date { get; set; }
    }
}
