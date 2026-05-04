using EDUBackEnd.Interfaces.Email;
using EDUBackEnd.Interfaces.Notification;
using EDUBackEnd.Models.Notificaiton;

namespace EDUBackEnd.Services.Notification
{
    public class NotificationSender : INotificationSender
    {
        private readonly IForgotPassword _emailService;
        public NotificationSender(IForgotPassword emailService)
        {
            _emailService = emailService;
        }
        public async Task SendAsync(Notifications notifications)
        {
            switch (notifications.Type)
            {
                case Enums.NotificationType.Email:
                    await SendEmail(notifications);
                    break;
                case Enums.NotificationType.Push:
                    await SendPush(notifications);
                    break;
            }
        }
        private Task SendEmail(Notifications n)
        {
            _emailService.SendEmailAsync(n.Email,n.Title ,n.Message);
            return Task.CompletedTask;
        }
        private Task SendPush(Notifications n)
        {
            Console.WriteLine($"Push Sent: {n.Message}");
            return Task.CompletedTask;
        }
    }
}
