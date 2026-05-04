using EDUBackEnd.Models.Attachment;

namespace EDUBackEnd.Interfaces.Attachment
{
    public interface IAttachmentService
    {
        Task<Guid> UploadAsync(IFormFile file);
        Task<(Stream Stream, Attachments Meta)> DownloadAsync(Guid id);
        Task DeleteAsync(Guid attachmentId);
    }
}
