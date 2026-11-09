using Blogy.Entity.Entities.Common;

namespace Blogy.Entity.Entities;

public class Contact : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;

    public string? AiResponse { get; set; }
    public string? DetectedLanguage { get; set; }
}