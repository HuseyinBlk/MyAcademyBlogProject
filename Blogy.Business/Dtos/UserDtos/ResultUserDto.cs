using Blogy.Entity.Entities;

namespace Blogy.Business.Dtos.UserDtos;

public class ResultUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string ProfilImageUrl { get; set; }
    public IList<string> Roles { get; set; } =  new List<string>();
}
