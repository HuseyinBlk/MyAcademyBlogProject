using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Blogy.Business.Dtos.UserDtos;

public class EditProfileDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public IFormFile ImageFile { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    [Compare("NewPassword", ErrorMessage = "Yeni şifreler birbiriyle uyuşmuyor!")]
    public string ConfirmNewPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}