using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Dtos.Common;
using Blogy.Entity.Entities;

namespace Blogy.Business.Dtos.CategoryDtos;

public class ResultCategoryDto : BaseDto
{
    public string Name { get; set; }
    public IList<ResultBlogDto> Blogs { get; set; }

}
