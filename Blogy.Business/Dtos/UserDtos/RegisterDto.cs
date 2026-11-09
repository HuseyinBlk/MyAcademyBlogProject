using System.ComponentModel.DataAnnotations;
using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.UserDtos;

public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Şifreler birbiri ile uyumlu değil.")]
    public string ConfirmPassword { get; set; }
}
