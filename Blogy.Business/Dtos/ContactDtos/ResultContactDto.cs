using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.ContactDtos;

public class ResultContactDto : BaseDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsRead { get; set; }
    public string? AiResponse { get; set; }
    public string? DetectedLanguage { get; set; }
}
