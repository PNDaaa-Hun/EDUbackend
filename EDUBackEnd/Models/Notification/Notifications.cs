using EDUBackEnd.Enums;

namespace EDUBackEnd.Models.Notificaiton
{
    public class Notifications
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Receiver's user ID
        public string SenderId { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool IsSent { get; set; }
    }
}
