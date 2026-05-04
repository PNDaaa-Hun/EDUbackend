using EDUBackEnd.Dtos.Adding;

namespace EDUBackEnd.Interfaces.Attachment
{
    public interface IBookService
    {
        Task<Guid> CreateAsync(AddBookDto dto);
        Task<BookDto> GetByIdAsync(Guid id);
        Task<List<BookDto>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}
