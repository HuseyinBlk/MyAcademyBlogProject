using AutoMapper;
using Blogy.Business.Dtos.SocialDtos;
using Blogy.Business.Validators.SocialValidators;
using Blogy.DataAccess.Repositories.SocialRepositories;
using Blogy.Entity.Entities;
using FluentValidation;

namespace Blogy.Business.Services.SocialServices;

public class SocialService(ISocialRepository socialRepository, IMapper mapper, CreateSocialValidator createValidator, UpdateSocialValidator updateValidator) : ISocialService
{
    public async Task<List<ResultSocialDto>> GetAllAsync()
    {

        var socials = await socialRepository.GetAllAsync();
        return mapper.Map<List<ResultSocialDto>>(socials);
    }

    public async Task<UpdateSocialDto> GetByIdAsync(int id)
    {
        var social = await socialRepository.GetByIdAsync(id);
        return mapper.Map<UpdateSocialDto>(social);
    }

    public async Task CreateAsync(CreateSocialDto entityDto)
    {
        var validationResult = await createValidator.ValidateAsync(entityDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var social = mapper.Map<Social>(entityDto);
        await socialRepository.CreateAsync(social);

    }

    public async Task UpdateAsync(UpdateSocialDto entityDto)
    {
        var validationResult = await updateValidator.ValidateAsync(entityDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var social = await socialRepository.GetByIdAsync(entityDto.Id);
        mapper.Map(entityDto, social);
        await socialRepository.UpdateAsync(social);

    }

    public async Task DeleteAsync(int id)
    {
        await socialRepository.DeleteAsync(id);
    }
}
