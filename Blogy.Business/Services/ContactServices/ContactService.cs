using AutoMapper;
using Blogy.Business.Dtos.ContactDtos;
using Blogy.Business.Services.AiServices;
using Blogy.Business.Services.EmailServices;
using Blogy.DataAccess.Repositories.ContactRepositories;
using Blogy.Entity.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blogy.Business.Services.ContactServices;

public class ContactService(
    IContactRepository contactRepository,
    IMapper mapper,
    IValidator<CreateContactDto> createValidator,
    IServiceScopeFactory scopeFactory,
    ILogger<ContactService> logger) : IContactService
{
    public async Task<List<ResultContactDto>> GetAllAsync()
    {
        var contacts = await contactRepository.GetAllAsync();
        return mapper.Map<List<ResultContactDto>>(contacts);
    }

    public async Task<ResultContactDto> GetByIdAsync(int id)
    {
        var contact = await contactRepository.GetByIdAsync(id);
        return mapper.Map<ResultContactDto>(contact);
    }

    public async Task CreateAsync(CreateContactDto dto)
    {
        var validationResult = await createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.CreateAsync(contact);

        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var scopedAiService = scope.ServiceProvider.GetRequiredService<IAiService>();
                var scopedEmailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var scopedTemplateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
                var scopedContactRepo = scope.ServiceProvider.GetRequiredService<IContactRepository>();

                var aiResult = await scopedAiService.GenerateReplyForContactAsync(dto.Message);

                var dbContact = await scopedContactRepo.GetByIdAsync(contact.Id);
                if (dbContact != null)
                {
                    dbContact.AiResponse = aiResult.Reply;
                    dbContact.DetectedLanguage = aiResult.DetectedLanguage;
                    await scopedContactRepo.UpdateAsync(dbContact);
                }

                if (!string.IsNullOrWhiteSpace(contact.Email) && !string.IsNullOrWhiteSpace(aiResult.Reply))
                {
                    var subject = $"Blogy - Mesajınız Alındı (Tespit Edilen Dil: {aiResult.DetectedLanguage})";
                    var emailBody = await scopedTemplateService.GetContactReplyTemplateAsync(
                        contact.Name,
                        aiResult.Reply,
                        aiResult.DetectedLanguage
                    );

                    await scopedEmailService.SendEmailAsync(contact.Email, subject, emailBody, isHtml: true);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Arka planda AI yanıtı üretilirken veya e-posta gönderilirken hata oluştu.");
            }
        });
    }

    public async Task DeleteAsync(int id)
    {
        await contactRepository.DeleteAsync(id);
    }
}
