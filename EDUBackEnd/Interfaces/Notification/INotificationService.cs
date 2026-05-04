using EDUBackEnd.Models.Notificaiton;

namespace EDUBackEnd.Interfaces.Notification
{
    public interface INotificationService
    {
        Task SendAsync(Notifications notifications);
    }
}
