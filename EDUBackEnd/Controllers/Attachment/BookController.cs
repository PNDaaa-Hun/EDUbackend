using EDUBackEnd.Dtos.Adding;
using EDUBackEnd.Interfaces.Attachment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDUBackEnd.Controllers.Attachment
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpPost("books/create")]
        public async Task<IActionResult> Create([FromBody] AddBookDto dto)
        {
            var id = await _bookService.CreateAsync(dto);
            return Ok(id);
        }
        [HttpGet("books/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _bookService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookService.GetAllAsync());
        }
        [HttpDelete("books/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();

        }
    }
}
