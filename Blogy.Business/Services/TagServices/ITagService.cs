using Blogy.Business.Dtos.TagDtos;

namespace Blogy.Business.Services.TagServices;

public interface ITagService
{
    Task<List<ResultTagDto>> GetAllAsync();
    Task<ResultTagDto> GetByIdAsync(int id);
    Task CreateAsync(CreateTagDto tagDto);
    Task UpdateAsync(UpdateTagDto tagDto);
    Task DeleteAsync(int id);
}
