namespace Tetas.Domain.Entities
{
    using System;

    public class Report
    {
        public long Id { get; set; }

        public string ReporterId { get; set; }

        public ApplicationUser Reporter { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public string Reason { get; set; }

        public bool Resolved { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
