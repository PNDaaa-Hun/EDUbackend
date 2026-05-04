using EDUBackEnd.Interfaces.Notification;

namespace EDUBackEnd.Services.Notification
{
    public class NotificationWorker : BackgroundService
    {
        private readonly INotificationQueue _queue;
        private readonly IServiceProvider _serviceProvider;
        public NotificationWorker(INotificationQueue queue, IServiceProvider serviceProvider)
        {
            _queue = queue;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var notification = _queue.Dequeue();
                if (notification != null)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notificationService.SendAsync(notification);
                    }
                }
                else
                {
                    await Task.Delay(500); // Wait before checking the queue again
                }
            }
        }
    }
}
