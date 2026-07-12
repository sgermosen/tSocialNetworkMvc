namespace Tetas.Repositories.Implementations
{
    using Contracts;
    using Domain.Entities;
    using Infraestructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ModerationRepository : IModeration
    {
        private readonly ApplicationDbContext _context;

        public ModerationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<string>> GetHiddenUserIdsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new HashSet<string>();
            }

            var blockedByMe = await _context.UserBlocks
                .Where(b => b.BlockerId == userId)
                .Select(b => b.BlockedId)
                .ToListAsync();

            var blockedMe = await _context.UserBlocks
                .Where(b => b.BlockedId == userId)
                .Select(b => b.BlockerId)
                .ToListAsync();

            return blockedByMe.Concat(blockedMe).ToHashSet();
        }

        public async Task<bool> BlockAsync(string blockerId, string blockedId)
        {
            if (string.IsNullOrEmpty(blockerId) || string.IsNullOrEmpty(blockedId) || blockerId == blockedId)
            {
                return false;
            }

            var exists = await _context.UserBlocks
                .AnyAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
            if (exists)
            {
                return true;
            }

            _context.UserBlocks.Add(new UserBlock
            {
                BlockerId = blockerId,
                BlockedId = blockedId,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnblockAsync(string blockerId, string blockedId)
        {
            var block = await _context.UserBlocks
                .FirstOrDefaultAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
            if (block == null)
            {
                return false;
            }

            _context.UserBlocks.Remove(block);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsBlockedAsync(string blockerId, string blockedId)
        {
            return await _context.UserBlocks
                .AnyAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
        }

        public async Task<List<ApplicationUser>> GetBlockedUsersAsync(string blockerId)
        {
            return await _context.UserBlocks
                .Where(b => b.BlockerId == blockerId)
                .Select(b => b.Blocked)
                .ToListAsync();
        }

        public async Task<bool> ReportPostAsync(string reporterId, long postId, string reason)
        {
            var already = await _context.Reports
                .AnyAsync(r => r.ReporterId == reporterId && r.PostId == postId);
            if (already)
            {
                return true;
            }

            _context.Reports.Add(new Report
            {
                ReporterId = reporterId,
                PostId = postId,
                Reason = reason,
                Resolved = false,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
