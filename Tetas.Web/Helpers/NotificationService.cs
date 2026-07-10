namespace Tetas.Web.Helpers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Tetas.Domain.Entities;
    using Tetas.Infraestructure;
    using Tetas.Web.Hubs;

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task NotifyAsync(string recipientId, string actorName, string message, string url)
        {
            if (string.IsNullOrEmpty(recipientId))
            {
                return;
            }

            var notification = new Notification
            {
                RecipientId = recipientId,
                ActorName = actorName,
                Message = message,
                Url = url,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            var unread = await _context.Notifications
                .CountAsync(n => n.RecipientId == recipientId && !n.IsRead);

            await _hub.Clients.User(recipientId).SendAsync("notify", new
            {
                id = notification.Id,
                actorName,
                message,
                url,
                createdAt = notification.CreatedAt,
                unread
            });
        }
    }
}
