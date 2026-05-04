namespace EDUBackEnd.Models.Attachment
{
    public class Attachments
    {
        public Guid Id { get; set; }
        public required string OriginalFileName { get; set; }
        public required string StoredFileName { get; set; }

        public required string Extension { get; set; }
        public required string ContentType { get; set; }

        public long Size { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
