using EDUBackEnd.Models.Notificaiton;

namespace EDUBackEnd.Interfaces.Notification
{
    public interface INotificationSender
    {
        Task SendAsync(Notifications notifications);
    }
}
