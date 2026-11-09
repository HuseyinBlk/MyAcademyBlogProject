using Blogy.Business.Dtos.CommentDtos;
using Blogy.Business.Services.Common;

namespace Blogy.Business.Services.CommentServices;

public interface ICommentService : IGenericService<ResultCommentDto,UpdateCommentDto,CreateCommentDto>
{
    public Task<List<ResultCommentDto>> GetCommentsWithBlogAndUserAsync();
    public Task<List<ResultCommentDto>> GetCommentsByUserIdAsync(int userId);
    Task ToggleCommentStatusAsync(int id);
    Task<bool> ReanalyzeWithHuggingFaceAsync(int id);
    Task<bool> ReanalyzeWithGeminiAsync(int id);

}
