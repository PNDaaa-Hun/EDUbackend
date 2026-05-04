using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Interfaces.Attachment;
using EDUBackEnd.Models.BookDomain;
using Microsoft.EntityFrameworkCore;

namespace EDUBackEnd.Services.Attachment
{
    public class BookService : IBookService
    {
        private readonly IAttachmentService _attachmentService;
        private readonly AppDbContext _context;
        public BookService(IAttachmentService attachmentService,
            AppDbContext context)
        {
            _attachmentService = attachmentService;
            _context = context;
        }

        public async Task<Guid> CreateAsync(AddBookDto dto)
        {
            var attachmentExists =await _context.Attachments.AnyAsync(a => a.Id == dto.AttachmentId);
            if (!attachmentExists)
                throw new ArgumentException("Attachment not found");
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                AttachmentId = dto.AttachmentId,
                Format = dto.Format,
                CreatedAt = DateTime.UtcNow

            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            
            return book.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id)
                ?? throw new ArgumentException("Book not found");
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BookDto>> GetAllAsync()
        {
            return await _context.Books
                .AsNoTracking()
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    AttachmentId = b.AttachmentId,
                    Format = b.Format,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<BookDto> GetByIdAsync(Guid id)
        {
            var book = await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id)
                ?? throw new ArgumentException("Book not found");
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                AttachmentId = book.AttachmentId,
                Format = book.Format,
                CreatedAt = book.CreatedAt
            };
        }
    }
}
