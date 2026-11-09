using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Dtos.Common;
using Blogy.Business.Dtos.UserDtos;

namespace Blogy.Business.Dtos.CommentDtos;

public class UpdateCommentDto : BaseDto
{
    public string Content { get; set; }

    public int BlogId { get; set; }

    public int UserId { get; set; }
}
