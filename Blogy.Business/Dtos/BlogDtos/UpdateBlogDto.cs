using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.BlogDtos;

public class UpdateBlogDto :BaseDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public string? BlogImage { get; set; }
    public string? BlogImage2 { get; set; }

    public int CategoryId { get; set; }
}