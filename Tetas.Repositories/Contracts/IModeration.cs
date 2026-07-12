namespace Tetas.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tetas.Domain.Entities;

    public interface IModeration
    {
        Task<HashSet<string>> GetHiddenUserIdsAsync(string userId);

        Task<bool> BlockAsync(string blockerId, string blockedId);

        Task<bool> UnblockAsync(string blockerId, string blockedId);

        Task<bool> IsBlockedAsync(string blockerId, string blockedId);

        Task<List<ApplicationUser>> GetBlockedUsersAsync(string blockerId);

        Task<bool> ReportPostAsync(string reporterId, long postId, string reason);
    }
}
