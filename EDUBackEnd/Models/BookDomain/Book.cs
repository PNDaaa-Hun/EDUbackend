using EDUBackEnd.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUBackEnd.Models.BookDomain
{
    public class Book
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Guid AttachmentId { get; set; }
        public BookFormat Format { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
