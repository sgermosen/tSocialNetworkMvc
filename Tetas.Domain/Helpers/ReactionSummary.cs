namespace Tetas.Domain.Helpers
{
    using System.Collections.Generic;

    public class ReactionSummary
    {
        public int Total { get; set; }

        public ReactionType? MyReaction { get; set; }

        public Dictionary<ReactionType, int> ByType { get; set; } = new Dictionary<ReactionType, int>();
    }
}
