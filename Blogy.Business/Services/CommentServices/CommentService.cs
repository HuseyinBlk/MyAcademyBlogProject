using AutoMapper;
using Blogy.Business.Dtos.CommentDtos;
using Blogy.Business.Services.AiServices;
using Blogy.Business.Services.NlpServices;
using Blogy.DataAccess.Repositories.CommentRepositories;
using Blogy.Entity.Entities;
using FluentValidation;

namespace Blogy.Business.Services.CommentServices;

public class CommentService(ICommentRepository commentRepository,IAiService aiService,IHuggingFaceService huggingFaceService, IMapper mapper, IValidator<Comment> validator) : ICommentService
{
    public async Task<List<ResultCommentDto>> GetAllAsync()
    {
        var comments = await commentRepository.GetAllAsync();
        return mapper.Map<List<ResultCommentDto>>(comments);
    }

    public async Task<UpdateCommentDto> GetByIdAsync(int id)
    {
        var comment =  await commentRepository.GetByIdAsync(id);
        return mapper.Map<UpdateCommentDto>(comment);
    }

    public async Task CreateAsync(CreateCommentDto entityDto)
    {
        var comment = mapper.Map<Comment>(entityDto);
        var analysis = await huggingFaceService.AnalyzeToxicityAsync(comment.Content);
        comment.TranslatedText = analysis.TranslatedText;
        comment.ToxicityScore = Math.Round(analysis.ToxicityScore, 4);
        comment.IsToxic = analysis.IsToxic;
        var result = validator.Validate(comment);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
        await commentRepository.CreateAsync(comment);
    }

    public async Task UpdateAsync(UpdateCommentDto entityDto)
    {
        var comment = mapper.Map<Comment>(entityDto);
        var result = validator.Validate(comment);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
        await commentRepository.UpdateAsync(comment);
    }

    public async Task DeleteAsync(int id)
    {
        await commentRepository.DeleteAsync(id);
    }

    public async Task<List<ResultCommentDto>> GetCommentsWithBlogAndUserAsync()
    {
        var comments = await  commentRepository.GetCommentsWithBlogAndUserAsync();
        return mapper.Map<List<ResultCommentDto>>(comments);
    }

    public async Task<List<ResultCommentDto>> GetCommentsByUserIdAsync(int userId)
    {
        var comments = await commentRepository.GetAllAsync(x => x.UserId == userId);
        return mapper.Map<List<ResultCommentDto>>(comments);
    }

    public async Task ToggleCommentStatusAsync(int id)
    {
        var comment = await commentRepository.GetByIdAsync(id);
        if(comment != null)
        {
            comment.IsToxic = !comment.IsToxic;
            await commentRepository.UpdateAsync(comment);
        }
    }

    public async Task<bool> ReanalyzeWithHuggingFaceAsync(int id)
    {
        var comment = await commentRepository.GetByIdAsync(id);
        if (comment == null) return false;
        var analysis = await huggingFaceService.AnalyzeToxicityAsync(comment.Content);
        comment.TranslatedText = analysis.TranslatedText;
        comment.ToxicityScore = Math.Round(analysis.ToxicityScore, 4);
        comment.IsToxic = analysis.IsToxic;
        await commentRepository.UpdateAsync(comment);
        return analysis.ToxicityScore >= 0;
    }
    public async Task<bool> ReanalyzeWithGeminiAsync(int id)
    {
        var comment = await commentRepository.GetByIdAsync(id);
        if (comment == null) return false;
        var analysis = await aiService.AnalyzeToxicityWithGeminiAsync(comment.Content);
        comment.TranslatedText = analysis.TranslatedText;
        comment.ToxicityScore = Math.Round(analysis.ToxicityScore, 4);
        comment.IsToxic = analysis.IsToxic;
        await commentRepository.UpdateAsync(comment);
        return analysis.ToxicityScore >= 0;
    }
}
