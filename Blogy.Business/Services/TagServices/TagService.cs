using AutoMapper;
using Blogy.Business.Dtos.TagDtos;
using Blogy.Business.Services.TagServices;
using Blogy.DataAccess.Repositories.TagRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.TagServices;

public class TagService(ITagRepository tagRepository, IMapper mapper) : ITagService
{
    public async Task<List<ResultTagDto>> GetAllAsync()
    {
        var tags = await tagRepository.GetAllAsync();
        return mapper.Map<List<ResultTagDto>>(tags);
    }

    public async Task<ResultTagDto> GetByIdAsync(int id)
    {
        var tag = await tagRepository.GetByIdAsync(id);
        return mapper.Map<ResultTagDto>(tag);
    }

    public async Task CreateAsync(CreateTagDto tagDto)
    {
        var tag = mapper.Map<Tag>(tagDto);
        await tagRepository.CreateAsync(tag);
    }

    public async Task UpdateAsync(UpdateTagDto tagDto)
    {
        var tag = await tagRepository.GetByIdAsync(tagDto.Id);
        mapper.Map(tagDto, tag);
        await tagRepository.UpdateAsync(tag);
    }

    public async Task DeleteAsync(int id)
    {
        await tagRepository.DeleteAsync(id);
    }
}
