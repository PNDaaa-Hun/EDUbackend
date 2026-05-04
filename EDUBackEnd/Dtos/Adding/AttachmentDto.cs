namespace EDUBackEnd.Dtos.Adding
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = default!;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = default!;
    }
}
