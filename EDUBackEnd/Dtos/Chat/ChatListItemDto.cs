namespace EDUBackEnd.Dtos.Chat
{
    public class ChatListItemDto
    {
        public string UserId { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
        public bool IsSeen { get; set; }
    }
}
