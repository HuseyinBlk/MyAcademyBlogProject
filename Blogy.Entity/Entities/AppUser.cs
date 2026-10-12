using Microsoft.AspNetCore.Identity;

namespace Blogy.Entity.Entities;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Title { get; set; }
    public string? ProfilImageUrl { get; set; }
}