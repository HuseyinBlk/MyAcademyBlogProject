namespace Blogy.Business.Configurations;

public class EmailOptions
{
    public const string Section = "EmailSettings";
    public string SmtpServer { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderPassword { get; set; } = string.Empty;
    public string SenderDisplayName { get; set; } = "Blogy Destek Ekibi";
    public bool EnableSsl { get; set; } = true;
}
