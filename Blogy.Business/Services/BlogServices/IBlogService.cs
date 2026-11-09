using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Services.Common;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.BlogServices;

public interface IBlogService : IGenericService<ResultBlogDto,UpdateBlogDto,CreateBlogDto>
{
    Task<List<ResultBlogDto>> GetBlogsWithCategoriesAsync();
    Task<List<ResultBlogDto>> GetBlogsByCategoryIdAsync(int categoryId);
    Task<List<ResultBlogDto>> GetLast3BlogsAsync();
    Task<List<ResultBlogDto>> GetLast5BlogsAsync();
    Task<ResultBlogDto> GetBlogDetailsByIdAsync(int blogId);
    Task<List<ResultBlogDto>> GetBlogByWriterIdAsync(int writerId);
    Task<List<ResultBlogDto>> GetBlogsByTagIdAsync(int tagId);
}
