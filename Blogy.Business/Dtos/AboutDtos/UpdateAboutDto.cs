using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.AboutDtos;

public class UpdateAboutDto : BaseDto
{
    public string Title1 { get; set; }
    public string Description1 { get; set; }
    public string? ImageUrl1 { get; set; }

    public string Title2 { get; set; }
    public string Description2 { get; set; }
    public string? ImageUrl2 { get; set; }

    public string FeatureTitle1 { get; set; }
    public string FeatureDescription1 { get; set; }

    public string FeatureTitle2 { get; set; }
    public string FeatureDescription2 { get; set; }

    public string FeatureTitle3 { get; set; }
    public string FeatureDescription3 { get; set; }

    public string? BottomTitle { get; set; }
    public string? BottomDescription { get; set; }
    public string? BottomImageUrl { get; set; }
}
