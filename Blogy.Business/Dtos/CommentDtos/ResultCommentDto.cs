using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Dtos.Common;
using Blogy.Business.Dtos.UserDtos;

namespace Blogy.Business.Dtos.CommentDtos;

public class ResultCommentDto : BaseDto
{
    public string Content { get; set; }
    public int BlogId { get; set; }
    public ResultBlogDto Blog { get; set; }
    public int UserId { get; set; }
    public ResultUserDto User { get; set; }

    public bool IsToxic { get; set; } = false;
    public double ToxicityScore { get; set; } = 0.0;
    public string? TranslatedText { get; set; }
}
