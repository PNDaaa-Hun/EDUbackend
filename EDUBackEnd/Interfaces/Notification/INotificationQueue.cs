using EDUBackEnd.Models.Notificaiton;

namespace EDUBackEnd.Interfaces.Notification
{
    public interface INotificationQueue
    {
        void Enqueue(Notifications notification);
        Notifications Dequeue();
    }
}
