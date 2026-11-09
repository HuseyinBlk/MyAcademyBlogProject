namespace Blogy.Business.Dtos.UserDtos;

public class TopWriterDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ProfilImageUrl { get; set; }
    public int BlogCount { get; set; }
}
