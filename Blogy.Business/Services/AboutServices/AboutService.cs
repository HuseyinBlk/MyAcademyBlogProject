using AutoMapper;
using Blogy.Business.Dtos.AboutDtos;
using Blogy.Business.Dtos.UserDtos;
using Blogy.DataAccess.Repositories.AboutRepositories;
using Blogy.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blogy.Business.Services.AboutServices;

public class AboutService(IAboutRepository aboutRepository,UserManager<AppUser> userManager, IMapper mapper, IValidator<CreateAboutDto> createValidator, IValidator<UpdateAboutDto> updateValidator) : IAboutService
{
    public async Task<List<ResultAboutDto>> GetAllAsync()
    {
        var  abouts = await aboutRepository.GetAllAsync();
        return mapper.Map<List<ResultAboutDto>>(abouts);
    }

    public async Task<UpdateAboutDto> GetByIdAsync(int id)
    {
        var about = await aboutRepository.GetByIdAsync(id);
        return mapper.Map<UpdateAboutDto>(about);
    }

    public async Task CreateAsync(CreateAboutDto entityDto)
    {
        var validationResult = await createValidator.ValidateAsync(entityDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var about = mapper.Map<About>(entityDto);
        await aboutRepository.CreateAsync(about);
    }

    public async Task UpdateAsync(UpdateAboutDto entityDto)
    {
        var validationResult = await updateValidator.ValidateAsync(entityDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var about = await aboutRepository.GetByIdAsync(entityDto.Id);
        mapper.Map(entityDto, about);
        await aboutRepository.UpdateAsync(about);
    }

    public async Task DeleteAsync(int id)
    {
        await aboutRepository.DeleteAsync(id);
    }

    public async Task<List<TopWriterDto>> GetTop6WritersAsync()
    {
        var writers = await userManager.Users
            .Where(u => u.Blogs.Any())
            .OrderByDescending(x => x.Blogs.Count)
            .Take(6)
            .Select(u => new TopWriterDto
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Title = u.Title ?? string.Empty,
                ProfilImageUrl = u.ProfilImageUrl ?? "/blogy-1.0.0/images/person_1.jpg",
                BlogCount = u.Blogs.Count
            }).ToListAsync();

        return writers;
    }
}
