namespace EDUBackEnd.Models.Chat
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public int? EventId { get; set; }

        public bool IsSeen { get; set; } = false;
        public string? FileUrl { get; internal set; }
    }
}
