using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Dtos.UserDtos;

namespace Blogy.Business.Dtos.CommentDtos;

public class CreateCommentDto 
{
    public string Content { get; set; }
    
    public int BlogId { get; set; }
    
    public int UserId { get; set; }
}