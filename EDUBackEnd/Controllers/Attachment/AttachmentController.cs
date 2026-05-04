using EDUBackEnd.Interfaces.Attachment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDUBackEnd.Controllers.Attachment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }
        [HttpPost("attachments/upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _attachmentService.UploadAsync(file);
            return Ok(result);
        }

        [HttpGet("attachments/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var (stream, meta) = await _attachmentService.DownloadAsync(id);

            return File(
                stream,
                meta.ContentType,
                meta.OriginalFileName
            );
        }

        [HttpDelete("attachments/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _attachmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
