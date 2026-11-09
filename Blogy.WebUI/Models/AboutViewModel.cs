using Blogy.Business.Dtos.AboutDtos;
using Blogy.Business.Dtos.UserDtos;

namespace Blogy.WebUI.Models;

public class AboutViewModel
{
    public ResultAboutDto? About { get; set; }
    public List<TopWriterDto> TopWriters { get; set; } = [];
    public string? AiSummary { get; set; }
}
