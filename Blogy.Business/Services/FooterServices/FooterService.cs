using AutoMapper;
using Blogy.Business.Dtos.FooterDtos;
using Blogy.Business.Validators.FooterValidators;
using Blogy.DataAccess.Repositories.FooterRepositories;
using Blogy.Entity.Entities;
using FluentValidation;

namespace Blogy.Business.Services.FooterServices;

public class FooterService(
    IFooterRepository footerRepository,
    IMapper mapper,
    CreateFooterValidator createValidator,
    UpdateFooterValidator updateValidator
    ) : IFooterService
{
    public async Task<List<ResultFooterDto>> GetAllAsync()
    {
        var footers = await footerRepository.GetAllAsync();
        return mapper.Map<List<ResultFooterDto>>(footers);
    }

    public async Task<UpdateFooterDto> GetByIdAsync(int id)
    {
        var footer = await footerRepository.GetByIdAsync(id);
        return mapper.Map<UpdateFooterDto>(footer);
    }

    public async Task CreateAsync(CreateFooterDto entityDto)
    {
        var validationResult = createValidator.Validate(entityDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var footer = mapper.Map<Footer>(entityDto);
        await footerRepository.CreateAsync(footer);
    }

    public async Task UpdateAsync(UpdateFooterDto entityDto)
    {
        var validationResult = updateValidator.Validate(entityDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var footer = mapper.Map<Footer>(entityDto);
        mapper.Map(entityDto, footer);
        await footerRepository.UpdateAsync(footer);
    }

    public async Task DeleteAsync(int id)
    {
        await footerRepository.DeleteAsync(id);
    }

    public async Task<string> GetFooterSummaryAsync()
    {
        var footers = await footerRepository.GetAllAsync();
        var first = footers.FirstOrDefault();
        return first?.Description ?? "Blogy, modern yazılım teknolojileri ve zengin içerikleri okuyucularıyla buluşturan profesyonel bir blog platformudur.";
    }
}
