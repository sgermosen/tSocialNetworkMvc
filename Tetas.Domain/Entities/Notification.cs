namespace Tetas.Domain.Entities
{
    using System;

    public class Notification
    {
        public long Id { get; set; }

        public string RecipientId { get; set; }

        public ApplicationUser Recipient { get; set; }

        public string ActorName { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
