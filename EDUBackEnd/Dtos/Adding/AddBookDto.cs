using EDUBackEnd.Enums;

namespace EDUBackEnd.Dtos.Adding
{
    public class AddBookDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string Authors { get; set; }
        public Guid AttachmentId { get; set; }
        public BookFormat Format { get; set; }
    }
}
