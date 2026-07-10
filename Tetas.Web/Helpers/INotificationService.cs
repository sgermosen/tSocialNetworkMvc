namespace Tetas.Web.Helpers
{
    using System.Threading.Tasks;

    public interface INotificationService
    {
        Task NotifyAsync(string recipientId, string actorName, string message, string url);
    }
}
