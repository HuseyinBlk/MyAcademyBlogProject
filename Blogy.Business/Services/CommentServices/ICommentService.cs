using Blogy.Business.Dtos.CommentDtos;
using Blogy.Business.Services.Common;

namespace Blogy.Business.Services.CommentServices;

public interface ICommentService : IGenericService<ResultCommentDto,UpdateCommentDto,CreateCommentDto>
{
    public Task<List<ResultCommentDto>> GetCommentsWithBlogAndUserAsync();
}