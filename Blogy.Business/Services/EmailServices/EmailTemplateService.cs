using Microsoft.AspNetCore.Hosting;

namespace Blogy.Business.Services.EmailServices;

public class EmailTemplateService(IWebHostEnvironment webHostEnvironment) : IEmailTemplateService
{
    public async Task<string> GetContactReplyTemplateAsync(string name, string aiResponse, string detectedLanguage)
    {
        var templatePath = Path.Combine(webHostEnvironment.WebRootPath, "templates", "contact-reply.html");

        if (!File.Exists(templatePath))
        {

            return $"Merhaba {name},<br/><br/>{aiResponse}<br/><br/>Dil: {detectedLanguage}";
        }

        var html = await File.ReadAllTextAsync(templatePath);

        return html
            .Replace("{{Name}}", name)
            .Replace("{{AiResponse}}", aiResponse)
            .Replace("{{DetectedLanguage}}", detectedLanguage)
            .Replace("{{Year}}", DateTime.Now.Year.ToString());
    }
}
