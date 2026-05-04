namespace EDUBackEnd.Models.Attachment
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; } = default!;
        public string StoredFileName { get; set; } = default!;

        public string Extension { get; set; } = default!;
        public string ContentType { get; set; } = default!;

        public long Size { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
