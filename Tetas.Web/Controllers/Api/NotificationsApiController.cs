namespace Tetas.Web.Controllers.Api
{
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Tetas.Infraestructure;
    using Tetas.Web.Models.Api;

    [ApiController]
    [Route("api/notifications")]
    [Produces("application/json")]
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> Get()
        {
            var items = await _context.Notifications
                .Where(n => n.RecipientId == UserId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    ActorName = n.ActorName,
                    Message = n.Message,
                    Url = n.Url,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("unread-count")]
        public async Task<ActionResult<int>> UnreadCount()
        {
            var count = await _context.Notifications
                .CountAsync(n => n.RecipientId == UserId && !n.IsRead);
            return Ok(count);
        }

        [HttpPost("{id:long}/read")]
        public async Task<IActionResult> MarkRead(long id)
        {
            var item = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.RecipientId == UserId);
            if (item == null)
            {
                return NotFound();
            }

            item.IsRead = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllRead()
        {
            var items = await _context.Notifications
                .Where(n => n.RecipientId == UserId && !n.IsRead)
                .ToListAsync();
            foreach (var item in items)
            {
                item.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
