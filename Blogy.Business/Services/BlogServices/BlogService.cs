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

    public async Task<List<ResultBlogDto>> GetBlogsWithCategoriesAsync()
    {
        var blogs = await blogRepository.GetBlogsWithCategoriesAsync();
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }

    public async Task<List<ResultBlogDto>> GetBlogsByCategoryIdAsync(int categoryId)
    {
        var blogs = await blogRepository.GetAllAsync(x => x.CategoryId == categoryId);
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }

    public async Task<List<ResultBlogDto>> GetLast3BlogsAsync()
    {
        var blogs = await blogRepository.GetLast3BlogsAsync();
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }

    public async Task<ResultBlogDto> GetBlogDetailsByIdAsync(int blogId)
    {
        var blog = await blogRepository.GetByIdAsync(blogId);
        return mapper.Map<ResultBlogDto>(blog);
    }
}