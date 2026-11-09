using AutoMapper;
using Blogy.Business.Dtos.BlogDtos;
using Blogy.DataAccess.Repositories.BlogRepositories;
using Blogy.DataAccess.Repositories.BlogTagRepositories;
using Blogy.DataAccess.Repositories.TagRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.BlogServices;

public class BlogService(IBlogRepository blogRepository, IBlogTagRepository blogTagRepository,ITagRepository tagRepository, IMapper mapper) : IBlogService
{
    public async Task<List<ResultBlogDto>> GetAllAsync()
    {
        var blogs = await blogRepository.GetAllAsync();
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }

    public async Task<UpdateBlogDto> GetByIdAsync(int id)
    {
        var blog =  await blogRepository.GetByIdAsync(id);
        var dto = mapper.Map<UpdateBlogDto>(blog);

        var blogTags = await blogTagRepository.GetAllAsync(x => x.BlogId == id);
        if (blogTags.Any())
        {
            dto.Tags = string.Join(", ", blogTags.Select(x => x.Tag.Name).Where(x => !string.IsNullOrEmpty(x)));
        }

        return dto;
    }

    public async Task CreateAsync(CreateBlogDto blogDto)
    {
        var entity = mapper.Map<Blog>(blogDto);
        await blogRepository.CreateAsync(entity);

        if (!string.IsNullOrWhiteSpace(blogDto.Tags))
        {
            await AssignTaskToBlogAsync(entity.Id, blogDto.Tags);
        }
    }

    public async Task UpdateAsync(UpdateBlogDto blogDto)
    {
        var blog = await blogRepository.GetByIdAsync(blogDto.Id);
        mapper.Map(blogDto, blog);
        await blogRepository.UpdateAsync(blog);

        if (blogDto.Tags != null)
        {
            var existingBlogTags = await blogTagRepository.GetAllAsync(x => x.BlogId == blog.Id);
            foreach (var bt in existingBlogTags)
            {
                await blogTagRepository.DeleteAsync(bt.Id);
            }

            if (!string.IsNullOrWhiteSpace(blogDto.Tags))
            {
                await AssignTaskToBlogAsync(blog.Id, blogDto.Tags);
            }
        }
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

    public async Task<List<ResultBlogDto>> GetLast5BlogsAsync()
    {
        var blogs = await blogRepository.GetAllAsync();
        var last5 = blogs.OrderByDescending(x => x.Id).Take(5).ToList();
        return mapper.Map<List<ResultBlogDto>>(last5);
    }

    public async Task<ResultBlogDto> GetBlogDetailsByIdAsync(int blogId)
    {
        var blog = await blogRepository.GetByIdAsync(blogId);
        return mapper.Map<ResultBlogDto>(blog);
    }

    public async Task<List<ResultBlogDto>> GetBlogByWriterIdAsync(int writerId)
    {
        var blogs = await blogRepository.GetAllAsync(x => x.WriterId == writerId);
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }

    public async Task<List<ResultBlogDto>> GetBlogsByTagIdAsync(int tagId)
    {
        var blogTags = await blogTagRepository.GetAllAsync(x => x.TagId == tagId);
        var blogIds = blogTags.Select(x => x.BlogId).Distinct().ToList();

        if (!blogIds.Any())
        {
            return new List<ResultBlogDto>();
        }

        var blogs = await blogRepository.GetAllAsync(x => blogIds.Contains(x.Id));
        return mapper.Map<List<ResultBlogDto>>(blogs);
    }

    private async Task AssignTaskToBlogAsync(int blogId, string tagString)
    {
        var tagNames = tagString.Split(',',StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => x.Trim().ToLowerInvariant()).Distinct();

        foreach (var tagName in tagNames)
        {
            if(string.IsNullOrWhiteSpace(tagName)) continue;

            var tags = await tagRepository.GetAllAsync(t => t.Name.ToLower() ==  tagName);
            var tag = tags.FirstOrDefault();

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                await tagRepository.CreateAsync(tag);
            }

            await blogTagRepository.CreateAsync(new BlogTag { BlogId = blogId, TagId = tag.Id });
        }
    }
}
