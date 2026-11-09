using Blogy.Business.Dtos.ContactDtos;

namespace Blogy.Business.Services.ContactServices;

public interface IContactService
{
    Task<List<ResultContactDto>> GetAllAsync();
    Task<ResultContactDto> GetByIdAsync(int id);
    Task CreateAsync(CreateContactDto dto);
    Task DeleteAsync(int id);
}
