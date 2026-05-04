namespace EDUBackEnd.Dtos.Chat
{
    public class CreateMessageDto
    {
        public int EventId { get; set; }
        public string Text { get; set; }
        public string? FileUrl { get; set; }
    }
}
