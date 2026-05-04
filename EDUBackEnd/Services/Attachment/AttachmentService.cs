using EDUBackEnd.Data;
using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Interfaces.Attachment;
using EDUBackEnd.Models.Attachment;

namespace EDUBackEnd.Services.Attachment
{
    public class AttachmentService : IAttachmentService
    {
        private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".png", ".docx",".txt" };
        private const long MaxFileSize = 100 * 1024 * 1024; // 100 MB

        private readonly AppDbContext _db;
        private readonly string _storagePath;

        public AttachmentService(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _storagePath = Path.Combine(env.ContentRootPath, "Storage", "Attachments");

            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public async Task<Guid> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Empty file");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("File too large");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("File type not allowed");

            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(_storagePath, storedFileName);

            await using (var stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Models.Attachment.Attachments
            {
                Id = Guid.NewGuid(),
                OriginalFileName = Path.GetFileName(file.FileName),
                StoredFileName = storedFileName,
                Extension = extension,
                ContentType = file.ContentType,
                Size = file.Length,
                CreatedAt = DateTime.UtcNow
            };

            _db.Attachments.Add(attachment);
            await _db.SaveChangesAsync();

            return attachment.Id;
        }

        public async Task<(Stream Stream, Attachments Meta)> DownloadAsync(Guid id)
        {
            var attachment = await _db.Attachments.FindAsync(id)
                ?? throw new FileNotFoundException();

            var fullPath = Path.Combine(_storagePath, attachment.StoredFileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException();

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

            return (stream, attachment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var attachment = await _db.Attachments.FindAsync(id)
                ?? throw new FileNotFoundException();

            var fullPath = Path.Combine(_storagePath, attachment.StoredFileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            _db.Attachments.Remove(attachment);
            await _db.SaveChangesAsync();
        }

        
    }
}
