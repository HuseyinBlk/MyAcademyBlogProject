using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Services.Common;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.BlogServices;

public interface IBlogService : IGenericService<ResultBlogDto,UpdateBlogDto,CreateBlogDto>
{
    Task<List<ResultBlogDto>> GetAllBlogsWithCategoriesAsync();
}