using System.Net;
using System.Net.Mail;
using Blogy.Business.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blogy.Business.Services.EmailServices;

public class EmailService(
    IOptions<EmailOptions> emailOptions,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailOptions _options = emailOptions.Value;

    public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
    {
        if (string.IsNullOrWhiteSpace(_options.SenderEmail) || string.IsNullOrWhiteSpace(_options.SenderPassword))
        {
            logger.LogWarning("Email ayarları yapılandırılmamış (SenderEmail veya SenderPassword boş). Mail gönderimi atlandı.");
            return;
        }

        try
        {
            var username = string.IsNullOrWhiteSpace(_options.SmtpUsername) ? _options.SenderEmail : _options.SmtpUsername;

            using var client = new SmtpClient(_options.SmtpServer, _options.SmtpPort)
            {
                EnableSsl = _options.EnableSsl,
                Credentials = new NetworkCredential(username, _options.SenderPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.SenderEmail, _options.SenderDisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
            logger.LogInformation("Kullanıcıya otomatik e-posta başarıyla gönderildi: {ToEmail}", toEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "E-posta gönderimi sırasında hata oluştu: {ToEmail}", toEmail);
        }
    }
}
