using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.CategoryDtos;

public class UpdateCategoryDto : BaseDto
{
    public string Name { get; set; }
}