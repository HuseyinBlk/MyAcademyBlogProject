using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.TagDtos;

public class UpdateTagDto : BaseDto
{
    public string Name { get; set; } = null!;
}
