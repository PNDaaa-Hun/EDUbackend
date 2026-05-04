using EDUBackEnd.Interfaces.Notification;
using EDUBackEnd.Models.Notificaiton;

namespace EDUBackEnd.Services.Notification
{
    public class NotificationQueue : INotificationQueue
    {
        private readonly Queue<Notifications> _queue = new();
        public Notifications Dequeue()
        {
            return _queue.Count > 0 ? _queue.Dequeue() : null;
        }

        public void Enqueue(Notifications notification)
        {
            _queue.Enqueue(notification);
        }
    }
}
