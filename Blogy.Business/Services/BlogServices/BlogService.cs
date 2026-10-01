using AutoMapper;
using Blogy.Business.Dtos.BlogDtos;
using Blogy.DataAccess.Repositories.BlogRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.BlogServices;

public class BlogService(IBlogRepository blogRepository, IMapper mapper) : IBlogService
{
    public async Task<List<ResultBlogDto>> GetAllAsync()
    {
        var blogs = await blogRepository.GetAllAsync();
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }

    public async Task<UpdateBlogDto> GetByIdAsync(int id)
    {
        var blog =  await blogRepository.GetByIdAsync(id);
        return mapper.Map<UpdateBlogDto>(blog);
    }

    public async Task CreateAsync(CreateBlogDto blogDto)
    {
        var entity = mapper.Map<Blog>(blogDto);
        await blogRepository.CreateAsync(entity);
    }

    public async Task UpdateAsync(UpdateBlogDto blogDto)
    {
        var blog = await blogRepository.GetByIdAsync(blogDto.Id);
        mapper.Map(blogDto, blog);
        await blogRepository.UpdateAsync(blog);
    }

    public async Task DeleteAsync(int id)
    {
        await blogRepository.DeleteAsync(id);
    }

    public async Task<List<ResultBlogDto>> GetAllBlogsWithCategoriesAsync()
    {
        var blogs = await blogRepository.GetAllBlogsWithCategoriesAsync();
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }
}