using AutoMapper;
using Blogy.Business.Dtos.CommentDtos;
using Blogy.DataAccess.Repositories.CommentRepositories;
using Blogy.Entity.Entities;
using FluentValidation;

namespace Blogy.Business.Services.CommentServices;

public class CommentService(ICommentRepository commentRepository, IMapper mapper, IValidator<Comment> validator) : ICommentService
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
}