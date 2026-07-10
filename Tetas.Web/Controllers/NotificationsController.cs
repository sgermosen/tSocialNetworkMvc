namespace Tetas.Web.Controllers
{
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Tetas.Infraestructure;

    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;

        public NotificationsController(ApplicationDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        private async Task<string> CurrentUserIdAsync()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            return user?.Id;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var userId = await CurrentUserIdAsync();

            var items = await _context.Notifications
                .Where(n => n.RecipientId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(20)
                .Select(n => new
                {
                    id = n.Id,
                    actorName = n.ActorName,
                    message = n.Message,
                    url = n.Url,
                    isRead = n.IsRead,
                    createdAt = n.CreatedAt
                })
                .ToListAsync();

            var unread = items.Count(i => !i.isRead);

            return Json(new { unread, items });
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(long id)
        {
            var userId = await CurrentUserIdAsync();
            var item = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.RecipientId == userId);
            if (item == null)
            {
                return NotFound();
            }

            item.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllRead()
        {
            var userId = await CurrentUserIdAsync();
            var items = await _context.Notifications
                .Where(n => n.RecipientId == userId && !n.IsRead)
                .ToListAsync();
            foreach (var item in items)
            {
                item.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
