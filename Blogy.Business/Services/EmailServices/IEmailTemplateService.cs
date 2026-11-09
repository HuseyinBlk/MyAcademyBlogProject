namespace Blogy.Business.Services.EmailServices;

public interface IEmailTemplateService
{
    Task<string> GetContactReplyTemplateAsync(string name, string aiResponse, string detectedLanguage);
}
