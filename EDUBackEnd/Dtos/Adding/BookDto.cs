using EDUBackEnd.Enums;

namespace EDUBackEnd.Dtos.Adding
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Guid AttachmentId { get; set; }
        public BookFormat Format { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
