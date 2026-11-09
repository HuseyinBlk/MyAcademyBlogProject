using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.SocialDtos;

public class UpdateSocialDto : BaseDto
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Icon { get; set; }
}
